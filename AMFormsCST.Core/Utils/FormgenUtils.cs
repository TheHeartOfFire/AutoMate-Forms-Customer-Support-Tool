using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.DotFormgen;

namespace AMFormsCST.Core.Utils;

public class FormgenUtils : IFormgenUtils
{
    private readonly IFileSystem _fileSystem;
    private string? _filePath;

    public string? FileName => _filePath is null ? null : _fileSystem.GetFileName(_filePath);
    public DotFormgen? ParsedFormgenFile { get; set; }
    public FormgenUtilsProperties Properties { get; } = new();

    public bool HasChanged => !(_backupCopy is null || ParsedFormgenFile is null || _backupCopy.Equals(ParsedFormgenFile));
    public event EventHandler? FormgenFileChanged;
    private void OnFormgenFileChanged() => FormgenFileChanged?.Invoke(this, EventArgs.Empty);

    private DotFormgen? _backupCopy;

    public FormgenUtils(IFileSystem fileSystem, FormgenUtilsProperties properties)
    {
        _fileSystem = fileSystem;
        Properties = properties ?? throw new ArgumentNullException(nameof(properties));
    }

    public void OpenFile(string filePath)
    {
        if (!filePath.Contains("FormgenBackup"))
            _filePath = filePath;

        var xmlContent = _fileSystem.ReadAllText(filePath);
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlContent);
        if (xmlDoc.DocumentElement is null) throw new XmlException("The XML file is empty or missing a root element.");
        ParsedFormgenFile = new DotFormgen(xmlDoc.DocumentElement);
        _backupCopy = ParsedFormgenFile.Clone(); // Create a backup copy of the original file

        ParsedFormgenFile.PropertyChanged += (s, e) => OnFormgenFileChanged();

    }

    public void SaveFile(string filePath, DotFormgen? fileToSave = null)
    {
        var file = fileToSave is null ? ParsedFormgenFile : fileToSave;

        if (file is null) return;

        var xmlContent = file.GenerateXML();
        _fileSystem.WriteAllText(filePath, xmlContent);

        if (fileToSave is not null) return;

        CreateBackup();
        _backupCopy = ParsedFormgenFile;
    }

    public void CloseFile()
    {
        if (ParsedFormgenFile is not null) ParsedFormgenFile.PropertyChanged -= (s, e) => OnFormgenFileChanged();
        ParsedFormgenFile = null;
        _filePath = null;
        _backupCopy = null;
    }

    public void RenameFile(string newName, bool renameImage)
    {
        if (string.IsNullOrEmpty(_filePath) || ParsedFormgenFile is null)
        {
            return;
        }

        var directory = _fileSystem.GetDirectoryName(_filePath);
        if (string.IsNullOrEmpty(directory))
        {
            return;
        }

        var originalFileNameWithoutExt = _fileSystem.GetFileNameWithoutExtension(_filePath);
        var newFormgenPath = _fileSystem.CombinePath(directory, newName + ".formgen");

        // Rename the .formgen file
        _fileSystem.MoveFile(_filePath, newFormgenPath);
        _filePath = newFormgenPath;

        if (renameImage)
        {
            var imageExtension = ParsedFormgenFile.FormType == Format.Pdf ? ".pdf" : ".jpg";
            var originalImagePath = _fileSystem.CombinePath(directory, originalFileNameWithoutExt + imageExtension);
            var newImagePath = _fileSystem.CombinePath(directory, newName + imageExtension);

            if (_fileSystem.FileExists(originalImagePath))
            {
                _fileSystem.MoveFile(originalImagePath, newImagePath);
            }
        }
    }

    public void RegenerateUUID()
    {
        if (ParsedFormgenFile is null) return;
        ParsedFormgenFile.Settings.UUID = Guid.NewGuid().ToString();
        OnFormgenFileChanged();
    }

    public CodeLine[] GetCodeLines(CodeLineSettings.CodeType type)
    {
        return ParsedFormgenFile?.CodeLines.Where(cl => cl.Settings?.Type == type).ToArray() ?? [];
    }

    public CodeLine[] GetPrompts()
    {
        return GetCodeLines(CodeLineSettings.CodeType.PROMPT);
    }

    public void EditPrompts(CodeLine[] prompts)
    {
        if (ParsedFormgenFile is null || prompts is null) return;

        var originalPrompts = GetPrompts().ToDictionary(p => p.Settings?.Variable ?? string.Empty);

        foreach (var editedPrompt in prompts)
        {
            if (editedPrompt.Settings?.Variable is not null && originalPrompts.TryGetValue(editedPrompt.Settings.Variable, out var originalPrompt))
            {
                // Update properties of the original prompt object
                originalPrompt.Expression = editedPrompt.Expression;
                if (originalPrompt.PromptData is not null && editedPrompt.PromptData is not null)
                {
                    originalPrompt.PromptData.Choices = editedPrompt.PromptData.Choices;
                    originalPrompt.PromptData.Message = editedPrompt.PromptData.Message;
                }
            }
        }
    }

    public void ClonePrompt(CodeLine[] selected)
    {
        if (ParsedFormgenFile is null || selected is null || selected.Length == 0)
        {
            return;
        }

        var allPrompts = GetPrompts().ToList();
        int maxIndex = allPrompts.Count != 0 ? allPrompts.Max(p => p.Settings?.Order ?? 0) : 0;

        foreach (var promptToClone in selected)
        {
            maxIndex++;
            string? newName = $"Copy of {promptToClone.Settings?.Variable}";
            var newPrompt = new CodeLine(promptToClone, newName, maxIndex);
            ParsedFormgenFile.CodeLines.Add(newPrompt);
        }

        // Re-sort all codelines to maintain order by type and then by index
        ParsedFormgenFile.CodeLines = ParsedFormgenFile.CodeLines
            .OrderBy(cl => cl.Settings?.Type)
            .ThenBy(cl => cl.Settings?.Order)
            .ToList();
    }

    public void CopyPromptsTo(string fromFilePath)
    {
        if (ParsedFormgenFile is null) return;

        // Open the source file to get its prompts
        var sourceXmlContent = _fileSystem.ReadAllText(fromFilePath);
        var sourceDoc = new XmlDocument();
        sourceDoc.LoadXml(sourceXmlContent);
        if (sourceDoc.DocumentElement is null) return; // Can't copy from an empty file
        var sourceFormgen = new DotFormgen(sourceDoc.DocumentElement);
        var sourcePrompts = sourceFormgen.CodeLines.Where(cl => cl.Settings?.Type == CodeLineSettings.CodeType.PROMPT);

        // Get existing prompts in the target file
        var targetPrompts = GetPrompts().ToList();
        int maxIndex = targetPrompts.Any() ? targetPrompts.Max(p => p.Settings?.Order ?? 0) : 0;
        var targetPromptNames = new HashSet<string?>(targetPrompts.Select(p => p.Settings?.Variable));

        foreach (var sourcePrompt in sourcePrompts)
        {
            // Only add if a prompt with the same variable name doesn't already exist
            if (sourcePrompt.Settings?.Variable is not null && !targetPromptNames.Contains(sourcePrompt.Settings.Variable))
            {
                maxIndex++;
                var newPrompt = new CodeLine(sourcePrompt, sourcePrompt.Settings.Variable, maxIndex);
                ParsedFormgenFile.CodeLines.Add(newPrompt);
                targetPromptNames.Add(newPrompt.Settings?.Variable);
            }
        }
    }

    public void CreateBackup()
    {
        if (string.IsNullOrEmpty(_filePath) || _backupCopy is null) return;

        var backupDir = _fileSystem.CombinePath(IO.BackupPath, $"{_fileSystem.GetFileNameWithoutExtension(_filePath)}_{_backupCopy.Settings.UUID}");
        _fileSystem.CreateDirectory(backupDir);

        // Enforce retention policy
        var retentionQty = Properties.BackupRetentionQty;
        if (retentionQty > 0)
        {
            var existingBackups = _fileSystem.GetFiles(backupDir)
                                             .OrderByDescending(f => _fileSystem.GetLastWriteTime(f))
                                             .ToList();

            if (existingBackups.Count >= retentionQty)
            {
                var backupsToDelete = existingBackups.Skip((int)retentionQty - 1);
                foreach (var oldBackup in backupsToDelete)
                {
                    _fileSystem.DeleteFile(oldBackup);
                }
            }
        }

        // Create new backup
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        var backupFileName = $"{{{timestamp}}}.bak";
        var backupPath = _fileSystem.CombinePath(backupDir, backupFileName);

        SaveFile(backupPath, _backupCopy);
    }

    public void LoadBackup(string backupPath)
    {
        if (_fileSystem.FileExists(backupPath))
        {
            OpenFile(backupPath);
            OnFormgenFileChanged();
        }
    }
}
