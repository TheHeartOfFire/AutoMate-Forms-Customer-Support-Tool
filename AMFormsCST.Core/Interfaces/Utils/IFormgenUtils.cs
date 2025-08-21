using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;

namespace AMFormsCST.Core.Interfaces.Utils;
public interface IFormgenUtils
{
    string? FileName { get; }
    DotFormgen? ParsedFormgenFile { get; }

    void ClonePrompt(CodeLine[] selected);
    void CloseFile();
    void CopyPromptsTo(string fromFilePath);
    void CreateBackup();
    void EditPrompts(CodeLine[] prompts);
    CodeLine[] GetCodeLines(CodeLineSettings.CodeType type);
    CodeLine[] GetPrompts();
    void LoadBackup(string backupPath);
    void OpenFile(string filePath);
    void RenameFile(string newName, bool renameImage);
    void SaveFile(string filePath, DotFormgen? fileToSave = null);
    void RegenerateUUID();
    bool HasChanged { get; }
    event EventHandler? FormgenFileChanged;
}