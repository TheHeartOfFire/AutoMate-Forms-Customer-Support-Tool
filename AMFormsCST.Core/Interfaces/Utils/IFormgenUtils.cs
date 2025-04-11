using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;

namespace AMFormsCST.Core.Interfaces.Utils;
public interface IFormgenUtils
{
    string? FileName { get; }
    DotFormgen? ParsedFormgenFile { get; }

    void ClonePrompt(CodeLine[] selected);
    void CloseFile();
    void CopyPromptsTo(string fromFilePath);
    void CreateBackup(string uuid);
    void EditPrompts(CodeLine[] prompts);
    CodeLine[] GetCodeLines(CodeLineSettings.CodeType type);
    CodeLine[] GetPrompts();
    void LoadBackup(string backupPath);
    void OpenFile(string filePath);
    void RenameFile(string newName, bool hasImageFile, bool renameImage);
    void SaveFile(string filePath);
    void RegenerateUUID();
}