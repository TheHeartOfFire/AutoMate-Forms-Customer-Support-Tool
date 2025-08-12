using AMFormsCST.Core.Interfaces.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace AMFormsCST.Desktop.Services;

public class FileSystemService : IFileSystem
{
    public bool FileExists(string? path) => File.Exists(path);

    public string? GetDirectoryName(string? path) => Path.GetDirectoryName(path);

    public string? GetFileName(string? path) => Path.GetFileName(path);

    public string? GetFileNameWithoutExtension(string? path) => Path.GetFileNameWithoutExtension(path);

    public string CombinePath(string path1, string path2) => Path.Combine(path1, path2);

    public string ReadAllText(string path) => File.ReadAllText(path);

    public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);

    public void MoveFile(string sourceFileName, string destFileName) => File.Move(sourceFileName, destFileName);

    public void CreateDirectory(string path) => Directory.CreateDirectory(path);

    public IEnumerable<string> GetFiles(string path) => Directory.GetFiles(path);

    public DateTime GetLastWriteTime(string path) => File.GetLastWriteTime(path);

    public void DeleteFile(string path) => File.Delete(path);
}