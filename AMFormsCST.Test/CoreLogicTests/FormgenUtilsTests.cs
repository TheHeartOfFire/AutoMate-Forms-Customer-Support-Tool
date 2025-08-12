using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Utils;
using Moq;
using System.IO;
using System.Text;
using System.Xml;

namespace AMFormsCST.Test.CoreLogicTests;

public class FormgenUtilsTests
{
    private readonly Mock<IFileSystem> _mockFileSystem;
    private readonly FormgenUtilsProperties _formgenUtilsProperties;

    public FormgenUtilsTests()
    {
        _mockFileSystem = new Mock<IFileSystem>();
        _formgenUtilsProperties = new FormgenUtilsProperties();
    }

    [Fact]
    public void OpenFile_WithValidXml_ParsesFileAndSetsProperty()
    {
        // Arrange
        var fakeFilePath = "C:\\forms\\test.formgen";
        var fakeXmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<formDef version=""1"" publishedUUID=""some-guid"" legacyImport=""false"" totalPages=""1"" defaultPoints=""10"" missingSourceJpeg=""false"" duplex=""false"" maxAccessoryLines=""0"" prePrintedLaserForm=""false"">
  <title>My Test Form</title>
</formDef>";

        // Configure the mock file system to return our fake XML content when ReadAllText is called.
        _mockFileSystem.Setup(fs => fs.ReadAllText(fakeFilePath)).Returns(fakeXmlContent);
        var formgenUtils = new FormgenUtils(_mockFileSystem.Object, _formgenUtilsProperties);

        // Act
        formgenUtils.OpenFile(fakeFilePath);

        // Assert
        // Verify that the file system was asked to read the file.
        _mockFileSystem.Verify(fs => fs.ReadAllText(fakeFilePath), Times.Once);

        // Verify that the ParsedFormgenFile property is not null and was populated correctly.
        Xunit.Assert.NotNull(formgenUtils.ParsedFormgenFile);
        Xunit.Assert.Equal("My Test Form", formgenUtils.ParsedFormgenFile.Title);
        Xunit.Assert.Equal("some-guid", formgenUtils.ParsedFormgenFile.Settings.UUID);
    }

    [Fact]
    public void OpenFile_WithInvalidXml_ThrowsXmlException()
    {
        // Arrange
        var fakeFilePath = "C:\\forms\\invalid.formgen";
        var invalidXmlContent = "<formDef><title>Missing closing tag</formDef>"; // Malformed XML

        _mockFileSystem.Setup(fs => fs.ReadAllText(fakeFilePath)).Returns(invalidXmlContent);
        var formgenUtils = new FormgenUtils(_mockFileSystem.Object, _formgenUtilsProperties);

        // Act & Assert
        // Verify that attempting to open a malformed XML file throws an XmlException.
        Xunit.Assert.Throws<XmlException>(() => formgenUtils.OpenFile(fakeFilePath));
    }

    [Fact]
    public void SaveFile_WritesCorrectXmlToFileSystem()
    {
        // Arrange
        var fakeFilePath = "C:\\forms\\output.formgen";
        var formgenUtils = new FormgenUtils(_mockFileSystem.Object, _formgenUtilsProperties);

        // Create a DotFormgen object in memory to be "saved".
        var fileToSave = new Core.Types.FormgenUtils.FormgenFileStructure.DotFormgen
        {
            Title = "Saved Form",
            Settings = { UUID = "new-guid", Version = 2 }
        };
        formgenUtils.ParsedFormgenFile = fileToSave;

        // Use a string builder to capture the text that the mock file system "writes".
        var savedContent = new StringBuilder();
        _mockFileSystem.Setup(fs => fs.WriteAllText(fakeFilePath, It.IsAny<string>()))
                       .Callback<string, string>((path, content) => savedContent.Append(content));

        // Act
        formgenUtils.SaveFile(fakeFilePath);

        // Assert
        // Verify that the file system's WriteAllText method was called.
        _mockFileSystem.Verify(fs => fs.WriteAllText(fakeFilePath, It.IsAny<string>()), Times.Once);

        // Verify that the generated XML contains the correct data.
        var resultXml = savedContent.ToString();
        Xunit.Assert.Contains(@"<title>Saved Form</title>", resultXml);
        Xunit.Assert.Contains(@"publishedUUID=""new-guid""", resultXml);
    }

    [Fact]
    public void CloseFile_ResetsParsedFormgenFile_ToNull()
    {
        // Arrange
        var formgenUtils = new FormgenUtils(_mockFileSystem.Object, _formgenUtilsProperties);
        formgenUtils.ParsedFormgenFile = new Core.Types.FormgenUtils.FormgenFileStructure.DotFormgen();
        Xunit.Assert.NotNull(formgenUtils.ParsedFormgenFile); // Pre-condition check

        // Act
        formgenUtils.CloseFile();

        // Assert
        Xunit.Assert.Null(formgenUtils.ParsedFormgenFile);
    }
}