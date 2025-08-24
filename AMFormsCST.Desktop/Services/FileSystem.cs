using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace AMFormsCST.Desktop.Services;

public class FileSystem : IFileSystem
{
    private readonly ILogService? _logger;

    public FileSystem(ILogService? logger = null)
    {
        _logger = logger;
        _logger?.LogInfo("FileSystem initialized.");
    }

    public bool FileExists(string? path)
    {
        var exists = File.Exists(path);
        _logger?.LogDebug($"FileExists('{path}') => {exists}");
        return exists;
    }

    public string? GetDirectoryName(string? path)
    {
        var result = Path.GetDirectoryName(path);
        _logger?.LogDebug($"GetDirectoryName('{path}') => '{result}'");
        return result;
    }

    public string? GetFileName(string? path)
    {
        var result = Path.GetFileName(path);
        _logger?.LogDebug($"GetFileName('{path}') => '{result}'");
        return result;
    }

    public string? GetFileNameWithoutExtension(string? path)
    {
        var result = Path.GetFileNameWithoutExtension(path);
        _logger?.LogDebug($"GetFileNameWithoutExtension('{path}') => '{result}'");
        return result;
    }

    public string CombinePath(string path1, string path2)
    {
        var result = Path.Combine(path1, path2);
        _logger?.LogDebug($"CombinePath('{path1}', '{path2}') => '{result}'");
        return result;
    }

    public string ReadAllText(string path)
    {
        _logger?.LogInfo($"ReadAllText('{path}')");
        return File.ReadAllText(path);
    }

    public void WriteAllText(string path, string contents)
    {
        _logger?.LogInfo($"WriteAllText('{path}')");
        File.WriteAllText(path, contents);
    }

    public void MoveFile(string sourceFileName, string destFileName)
    {
        _logger?.LogInfo($"MoveFile('{sourceFileName}', '{destFileName}')");
        File.Move(sourceFileName, destFileName);
    }

    public void CreateDirectory(string path)
    {
        _logger?.LogInfo($"CreateDirectory('{path}')");
        Directory.CreateDirectory(path);
    }

    public IEnumerable<string> GetFiles(string path)
    {
        _logger?.LogInfo($"GetFiles('{path}')");
        return Directory.GetFiles(path);
    }

    public DateTime GetLastWriteTime(string path)
    {
        var result = File.GetLastWriteTime(path);
        _logger?.LogDebug($"GetLastWriteTime('{path}') => {result}");
        return result;
    }

    public void DeleteFile(string path)
    {
        _logger?.LogInfo($"DeleteFile('{path}')");
        File.Delete(path);
    }
}