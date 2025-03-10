using AMFormsCST.Core.Types.Notebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AMFormsCST.Core;
internal static class IO
{
    private static readonly string _appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static readonly string _rootPath = _appData + "\\AMFormsCST";
    private static readonly string _notesPath = _rootPath + "\\SavedNotes.json";
    internal static readonly string BackupPath = _rootPath + "\\FormgenBackup";

    internal static string BackupFormgenFilePath(string uuid) => $"{BackupPath}\\{uuid}\\{DateTime.Now:mm-dd-yyyy.hh-mm-ss}.bak";

    internal static void BackupFormgenFile(string uuid, XmlDocument file, uint? retentionCount)
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
    internal static string AutoIncrement(string? input)
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

    internal static void SaveNotes(List<Note> notes)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(notes);

        if (!Directory.Exists(_rootPath))
            Directory.CreateDirectory(_rootPath);

        File.WriteAllText(_notesPath, json);
    }
    internal static List<Note> LoadNotes()
    {
        if (!File.Exists(_notesPath))
            return [];

        var json = File.ReadAllText(_notesPath);

        return System.Text.Json.JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
    }

}
