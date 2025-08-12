namespace AMFormsCST.Core.Interfaces.Utils;

public interface IFileSystem
{
    bool FileExists(string? path);
    string? GetDirectoryName(string? path);
    string? GetFileName(string? path);
    string? GetFileNameWithoutExtension(string? path);
    string CombinePath(string path1, string path2);
    string ReadAllText(string path);
    void WriteAllText(string path, string contents);
    void MoveFile(string sourceFileName, string destFileName);
    void CreateDirectory(string path);
    IEnumerable<string> GetFiles(string path);
    DateTime GetLastWriteTime(string path);
    void DeleteFile(string path);
}