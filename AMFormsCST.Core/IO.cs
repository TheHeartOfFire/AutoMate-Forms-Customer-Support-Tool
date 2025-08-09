using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Interfaces.UserSettings;
using AMFormsCST.Core.Types;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Core.Types.Notebook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Xml;

namespace AMFormsCST.Core;
public static class IO
{
    private static readonly string _appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static readonly string _rootPath;
    private static readonly string _notesPath;
    private static readonly string _settingsPath;
    public static readonly string BackupPath;
    private static readonly string _templatesPath;
    private static JsonSerializerOptions _jsonOptions = new() { WriteIndented = true }; // Default options

    public static string BackupFormgenFilePath(string uuid) => $"{BackupPath}\\{uuid}\\{DateTime.Now:mm-dd-yyyy.hh-mm-ss}.bak";

    static IO()
    {
        _rootPath = Path.Combine(_appData, "Solera Case Management Tool");
        _notesPath = Path.Combine(_rootPath, "SavedNotes.json");
        _settingsPath = Path.Combine(_rootPath, "AppSettings.json");
        BackupPath = Path.Combine(_rootPath, "FormgenBackup");
        _templatesPath = Path.Combine(_rootPath, "TextTemplates.json");

        try
        {
            if (!Directory.Exists(_rootPath)) Directory.CreateDirectory(_rootPath);
            if (!Directory.Exists(BackupPath)) Directory.CreateDirectory(BackupPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating application directories: {ex.Message}");
        }
    }

    /// <summary>
    /// Configures the JSON serializer with options provided by the UI project.
    /// </summary>
    public static void ConfigureJson(JsonSerializerOptions options)
    {
        _jsonOptions = options;
    }

    public static void SaveSettings(ISettings settings)
    {
        try
        {
            var json = JsonSerializer.Serialize(settings, _jsonOptions);
            File.WriteAllText(_settingsPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving settings: {ex.Message}");
        }
    }

    public static ISettings? LoadSettings()
    {
        if (!File.Exists(_settingsPath))
        {
            return null;
        }

        try
        {
            var json = File.ReadAllText(_settingsPath);
            return JsonSerializer.Deserialize<ISettings>(json, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading settings: {ex.Message}");
            return null;
        }
    }

    public static void BackupFormgenFile(string uuid, XmlDocument file, uint? retentionCount)
    {
        var di = Directory.CreateDirectory($"{BackupPath}\\{uuid}");

        if(retentionCount is not null && di.EnumerateFiles().Count() > retentionCount)
        {
            var files = di.EnumerateFiles().OrderByDescending(x => x.LastWriteTime).Skip((int)retentionCount);
            foreach (var fileToDelete in files)
                fileToDelete.Delete();
        }

        file.Save(BackupFormgenFilePath(uuid));
    }
    public static string AutoIncrement(string? input)
    {
        if (input == null) return input ?? string.Empty;

        var index = input.Length - 1;
        while (int.TryParse(input[index].ToString(), out _))
        {
            index--;
        }

        _ = int.TryParse(input.AsSpan(index + 1), out int number);

        number++;
        var output = string.Concat(input.AsSpan(0, index + 1), number.ToString());
        return output;

    }

    public static void SaveNotes(List<INote> notes)
    {
        var concreteNotes = notes.Cast<Note>().ToList(); 

        var json = JsonSerializer.Serialize(concreteNotes, _jsonOptions);

        try
        {
            File.WriteAllText(_notesPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving Notes: {ex.Message}");
        }
    }
    public static List<INote> LoadNotes()
    {
        if (!File.Exists(_notesPath))
        {
            SaveNotes([]); 
            return [];
        }

        try
        {
            var json = File.ReadAllText(_notesPath);

            var deserializedConcreteNotes = JsonSerializer.Deserialize<List<Note>>(json, _jsonOptions);

            return deserializedConcreteNotes?.Cast<INote>().ToList() ?? [];
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing Notes: {ex.Message}");
            return [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading Notes file: {ex.Message}");
            return [];
        }
    }

    public static List<TextTemplate> LoadTemplates()
    {
        if (!File.Exists(_templatesPath))
        {
            SaveTemplates([]); 
            return []; 
        }

        try
        {
            var json = File.ReadAllText(_templatesPath);
            var templates = JsonSerializer.Deserialize<List<TextTemplate>>(json, _jsonOptions);

            return templates ?? []; 
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing TextTemplates: {ex.Message}");
            return []; 
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"Error loading TextTemplates file: {ex.Message}");
            return [];
        }
    }
    public static void SaveTemplates(List<TextTemplate> templates)
    {
        try
        {
            var json = JsonSerializer.Serialize(templates, _jsonOptions);

            File.WriteAllText(_templatesPath, json);
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"Error saving TextTemplates: {ex.Message}");
        }
    }
}
