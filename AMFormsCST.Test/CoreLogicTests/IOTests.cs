using AMFormsCST.Core;
using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Interfaces.UserSettings;
using AMFormsCST.Core.Types;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Core.Types.Notebook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml;
using Xunit;

public class IOTests : IDisposable
{
    private readonly string _testRoot;
    private readonly string _settingsPath;
    private readonly string _notesPath;
    private readonly string _templatesPath;

    public IOTests()
    {
        // Setup a unique test directory for isolation
        _testRoot = Path.Combine(Path.GetTempPath(), "AMFormsCST_IOTests_" + Guid.NewGuid());
        Directory.CreateDirectory(_testRoot);

        // Patch private static fields using reflection (for test isolation)
        typeof(IO).GetField("_rootPath", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(null, _testRoot);
        typeof(IO).GetField("_settingsPath", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(null, Path.Combine(_testRoot, "AppSettings.json"));
        typeof(IO).GetField("_notesPath", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(null, Path.Combine(_testRoot, "SavedNotes.json"));
        typeof(IO).GetField("_templatesPath", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
            ?.SetValue(null, Path.Combine(_testRoot, "TextTemplates.json"));
        typeof(IO).GetField("BackupPath", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
            ?.SetValue(null, Path.Combine(_testRoot, "FormgenBackup"));

        _settingsPath = Path.Combine(_testRoot, "AppSettings.json");
        _notesPath = Path.Combine(_testRoot, "SavedNotes.json");
        _templatesPath = Path.Combine(_testRoot, "TextTemplates.json");
    }

    [Fact]
    public void SaveAndLoadSettings_WritesAndReadsSettingsFile()
    {
        // Arrange
        var userSettings = new MockUserSettings();
        var uiSettings = new MockUiSettings();
        var settings = new Settings(userSettings, uiSettings);

        // Act
        IO.SaveSettings(settings);
        var loaded = IO.LoadSettings();

        // Assert
        Assert.NotNull(loaded);
        Assert.Equal(userSettings.Name, ((MockUserSettings)loaded.UserSettings).Name);
    }

    [Fact]
    public void SaveAndLoadNotes_WritesAndReadsNotesFile()
    {
        // Arrange
        var notes = new List<INote>
        {
            new Note { ServerId = "S1", NotesText = "Test Note" }
        };

        // Act
        IO.SaveNotes(notes);
        var loaded = IO.LoadNotes();

        // Assert
        Assert.Single(loaded);
        Assert.Equal("S1", loaded[0].ServerId);
        Assert.Equal("Test Note", loaded[0].NotesText);
    }

    [Fact]
    public void SaveAndLoadTemplates_WritesAndReadsTemplatesFile()
    {
        // Arrange
        var templates = new List<TextTemplate>
        {
            new TextTemplate("T1", "D1", "Text1")
        };

        // Act
        IO.SaveTemplates(templates);
        var loaded = IO.LoadTemplates();

        // Assert
        Assert.Single(loaded);
        Assert.Equal("T1", loaded[0].Name);
        Assert.Equal("D1", loaded[0].Description);
        Assert.Equal("Text1", loaded[0].Text);
    }

    [Fact]
    public void BackupFormgenFile_CreatesBackupFile()
    {
        // Arrange
        var uuid = Guid.NewGuid().ToString();
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml("<root><test>1</test></root>");
        var backupDir = Path.Combine(_testRoot, "FormgenBackup", uuid);

        // Act
        IO.BackupFormgenFile(uuid, xmlDoc, null);

        // Assert
        Assert.True(Directory.Exists(backupDir));
        var files = Directory.GetFiles(backupDir);
        Assert.Single(files);
        var content = File.ReadAllText(files[0]);
        Assert.Contains("<test>1</test>", content);
    }

    [Fact]
    public void AutoIncrement_IncrementsNumericSuffix()
    {
        // Arrange
        var input = "ABC123";

        // Act
        var result = IO.AutoIncrement(input);

        // Assert
        Assert.Equal("ABC124", result);
    }

    [Fact]
    public void AutoIncrement_ReturnsInputIfNoNumericSuffix()
    {
        // Arrange
        var input = "ABC";

        // Act
        var result = IO.AutoIncrement(input);

        // Assert
        Assert.Equal("ABC1", result);
    }

    public void Dispose()
    {
        // Clean up test directory
        if (Directory.Exists(_testRoot))
            Directory.Delete(_testRoot, true);
    }

    // Minimal mock implementations for test isolation
    private class MockUserSettings : IUserSettings
    {
        public string Name { get; set; } = "TestUser";
        public IOrgVariables Organization { get; set; } = null!;
        public string ApplicationFilesPath { get; set; } = "";
        public string ExtSeparator { get; set; } = "x";
    }

    private class MockUiSettings : IUiSettings
    {
        public string Theme { get; set; } = "Light";
        public List<ISetting> Settings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}