using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using Assert = Xunit.Assert;
using Alignment = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.FormFieldSettings.Alignment;
using FieldType = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.FormFieldSettings.FieldType;

namespace AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

public class FormFieldSettingsTests
{
    [Fact]
    public void ParameterlessConstructor_InitializesWithDefaults()
    {
        // Arrange & Act
        var settings = new FormFieldSettings();

        // Assert
        Assert.Equal(0, settings.ID);
        Assert.Equal(FieldType.TEXT, settings.Type); // Default from GetFieldType
        Assert.Equal(new Point(0, 0), settings.ImpactPosition);
        Assert.Equal(new Rectangle(0, 0, 0, 0), settings.LaserRect);
        Assert.False(settings.ManualSize);
        Assert.Equal(0, settings.FontSize);
        Assert.False(settings.Bold);
    }

    [Fact]
    public void XmlConstructor_ParsesAllAttributesCorrectly()
    {
        // Arrange
        var doc = new XmlDocument();
        var element = doc.CreateElement("value");
        element.SetAttribute("uniqueId", "123");
        element.SetAttribute("formFieldType", "NUMERIC");
        element.SetAttribute("legacyCol", "10");
        element.SetAttribute("legacyLine", "20");
        element.SetAttribute("x", "30");
        element.SetAttribute("y", "40");
        element.SetAttribute("w", "50");
        element.SetAttribute("h", "60");
        element.SetAttribute("manualSize", "true");
        element.SetAttribute("fontPoints", "12");
        element.SetAttribute("boldFont", "true");
        element.SetAttribute("shrinkFontToFit", "true");
        element.SetAttribute("pictureLeft", "5");
        element.SetAttribute("pictureRight", "2");
        element.SetAttribute("displayPartialField", "true");
        element.SetAttribute("startChar", "1");
        element.SetAttribute("endChar", "5");
        element.SetAttribute("perCharDeltaPts", "1");
        element.SetAttribute("alignment", "Center");
        var attributes = element.Attributes;

        // Act
        var settings = new FormFieldSettings(attributes);

        // Assert
        Assert.Equal(123, settings.ID);
        Assert.Equal(FieldType.NUMERIC, settings.Type);
        Assert.Equal(new Point(10, 20), settings.ImpactPosition);
        Assert.Equal(new Rectangle(30, 40, 50, 60), settings.LaserRect);
        Assert.True(settings.ManualSize);
        Assert.Equal(12, settings.FontSize);
        Assert.True(settings.Bold);
        Assert.True(settings.ShrinkToFit);
        Assert.Equal(5, settings.Length);
        Assert.Equal(2, settings.DecimalPlaces);
        Assert.True(settings.DisplayPartial);
        Assert.Equal(1, settings.StartIndex);
        Assert.Equal(5, settings.EndIndex);
        Assert.Equal(1, settings.Kearning);
        Assert.Equal(Alignment.CENTER, settings.FontAlignment);
    }

    [Theory]
    [InlineData("TEXT", FieldType.TEXT)]
    [InlineData("NUMERIC", FieldType.NUMERIC)]
    [InlineData("SIGNATURE", FieldType.SIGNATURE)]
    [InlineData("Initials", FieldType.INITIALS)]
    [InlineData("INVALID", FieldType.TEXT)] // Default case
    public void GetFieldType_FromString_ReturnsCorrectEnum(string typeString, FieldType expectedType)
    {
        // Act
        var result = FormFieldSettings.GetFieldType(typeString);
        // Assert
        Assert.Equal(expectedType, result);
    }

    [Theory]
    [InlineData(Alignment.LEFT, "Left")]
    [InlineData(Alignment.CENTER, "Center")]
    [InlineData(Alignment.RIGHT, "Right")]
    [InlineData((Alignment)99, "Left")] // Default case
    public void GetAlignment_FromEnum_ReturnsCorrectString(Alignment alignmentEnum, string expectedString)
    {
        // Act
        var result = FormFieldSettings.GetAlignment(alignmentEnum);
        // Assert
        Assert.Equal(expectedString, result);
    }

    [Fact]
    public void GenerateXml_WritesAllAttributesCorrectly()
    {
        // Arrange
        var settings = new FormFieldSettings
        {
            ID = 456,
            Type = FieldType.SIGNATURE,
            ImpactPosition = new Point(5, 15),
            LaserRect = new Rectangle(25, 35, 45, 55),
            ManualSize = true,
            FontSize = 14,
            Bold = true,
            ShrinkToFit = false,
            Length = 10,
            DecimalPlaces = 0,
            DisplayPartial = false,
            StartIndex = 0,
            EndIndex = 0,
            Kearning = 0,
            FontAlignment = Alignment.RIGHT
        };
        var sb = new StringBuilder();
        using var writer = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true });
        writer.WriteStartElement("dummy");

        // Act
        settings.GenerateXml(writer);
        writer.WriteEndElement();
        writer.Flush();
        var resultXml = sb.ToString();

        // Assert
        var expected = "<dummy uniqueId=\"456\" formFieldType=\"SIGNATURE\" legacyCol=\"5\" legacyLine=\"15\" x=\"25\" y=\"35\" w=\"45\" h=\"55\" manualSize=\"true\" fontPoints=\"14\" boldFont=\"true\" shrinkFontToFit=\"false\" pictureLeft=\"10\" pictureRight=\"0\" displayPartialField=\"false\" startChar=\"0\" endChar=\"0\" perCharDeltaPts=\"0\" alignment=\"Right\" />";
        Assert.Equal(expected, resultXml);
    }
}