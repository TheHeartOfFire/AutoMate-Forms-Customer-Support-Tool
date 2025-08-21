using AMFormsCST.Desktop.Services;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace AMFormsCST.Test.Desktop.Services;
public class FileSystemTests : IDisposable
{
    private readonly string _testDir;
    private readonly string _testFile;
    private readonly string _testFile2;

    public FileSystemTests()
    {
        _testDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDir);
        _testFile = Path.Combine(_testDir, "test.txt");
        _testFile2 = Path.Combine(_testDir, "test2.txt");
        File.WriteAllText(_testFile, "Hello World");
    }

    [Fact]
    public void FileExists_ReturnsTrueForExistingFile()
    {
        var fs = new FileSystem();
        Assert.True(fs.FileExists(_testFile));
        Assert.False(fs.FileExists(Path.Combine(_testDir, "notfound.txt")));
    }

    [Fact]
    public void GetDirectoryName_ReturnsDirectory()
    {
        var fs = new FileSystem();
        Assert.Equal(_testDir, fs.GetDirectoryName(_testFile));
    }

    [Fact]
    public void GetFileName_ReturnsFileName()
    {
        var fs = new FileSystem();
        Assert.Equal("test.txt", fs.GetFileName(_testFile));
    }

    [Fact]
    public void GetFileNameWithoutExtension_ReturnsFileNameWithoutExtension()
    {
        var fs = new FileSystem();
        Assert.Equal("test", fs.GetFileNameWithoutExtension(_testFile));
    }

    [Fact]
    public void CombinePath_CombinesPaths()
    {
        var fs = new FileSystem();
        var combined = fs.CombinePath(_testDir, "abc.txt");
        Assert.Equal(Path.Combine(_testDir, "abc.txt"), combined);
    }

    [Fact]
    public void ReadAllText_ReadsFileContent()
    {
        var fs = new FileSystem();
        Assert.Equal("Hello World", fs.ReadAllText(_testFile));
    }

    [Fact]
    public void WriteAllText_WritesFileContent()
    {
        var fs = new FileSystem();
        fs.WriteAllText(_testFile2, "Test Content");
        Assert.Equal("Test Content", File.ReadAllText(_testFile2));
    }

    [Fact]
    public void MoveFile_MovesFile()
    {
        var fs = new FileSystem();
        fs.MoveFile(_testFile, _testFile2);
        Assert.False(File.Exists(_testFile));
        Assert.True(File.Exists(_testFile2));
    }

    [Fact]
    public void CreateDirectory_CreatesDirectory()
    {
        var fs = new FileSystem();
        var newDir = Path.Combine(_testDir, "subdir");
        fs.CreateDirectory(newDir);
        Assert.True(Directory.Exists(newDir));
    }

    [Fact]
    public void GetFiles_ReturnsFiles()
    {
        var fs = new FileSystem();
        fs.WriteAllText(_testFile2, "Another");
        var files = fs.GetFiles(_testDir).ToList();
        Assert.Contains(_testFile2, files);
    }

    [Fact]
    public void GetLastWriteTime_ReturnsLastWriteTime()
    {
        var fs = new FileSystem();
        var lastWrite = fs.GetLastWriteTime(_testFile);
        Assert.True(lastWrite <= DateTime.Now);
    }

    [Fact]
    public void DeleteFile_DeletesFile()
    {
        var fs = new FileSystem();
        fs.WriteAllText(_testFile2, "DeleteMe");
        fs.DeleteFile(_testFile2);
        Assert.False(File.Exists(_testFile2));
    }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(_testDir))
                Directory.Delete(_testDir, true);
        }
        catch { /* ignore cleanup errors */ }
    }
}