using AMFormsCST.Desktop.Converters;
using System.Collections.Generic;
using System.Globalization;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Desktop.Converters;

public class StringListToStringConverterTests
{
    private readonly StringListToStringConverter _converter = new();

    [Fact]
    public void Convert_WithListOfStrings_ReturnsNewLineSeparatedString()
    {
        // Arrange
        var stringList = new List<string> { "line 1", "line 2", "line 3" };
        var expected = "line 1\r\nline 2\r\nline 3";

        // Act
        var result = _converter.Convert(stringList, typeof(string), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_WithEmptyList_ReturnsEmptyString()
    {
        // Arrange
        var emptyList = new List<string>();

        // Act
        var result = _converter.Convert(emptyList, typeof(string), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Convert_WithNullValue_ReturnsEmptyString()
    {
        // Arrange & Act
        var result = _converter.Convert(null, typeof(string), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ConvertBack_WithNewLineSeparatedString_ReturnsListOfStrings()
    {
        // Arrange
        var text = "line 1\r\nline 2\r\nline 3";
        var expected = new List<string> { "line 1", "line 2", "line 3" };

        // Act
        var result = _converter.ConvertBack(text, typeof(List<string>), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertBack_WithEmptyString_ReturnsEmptyList()
    {
        // Arrange
        var text = string.Empty;

        // Act
        var result = _converter.ConvertBack(text, typeof(List<string>), null, CultureInfo.CurrentCulture) as List<string>;

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void ConvertBack_WithNullValue_ReturnsEmptyList()
    {
        // Arrange & Act
        var result = _converter.ConvertBack(null, typeof(List<string>), null, CultureInfo.CurrentCulture) as List<string>;

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}