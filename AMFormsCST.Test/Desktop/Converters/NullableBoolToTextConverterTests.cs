using AMFormsCST.Desktop.Converters;
using System;
using System.Globalization;
using Xunit;

namespace AMFormsCST.Test.Desktop.Converters;
public class NullableBoolToTextConverterTests
{
    private readonly NullableBoolToTextConverter _converter = new();

    [Fact]
    public void Convert_True_ReturnsImageFound()
    {
        // Act
        var result = _converter.Convert(true, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal("Image Found", result);
    }

    [Fact]
    public void Convert_False_ReturnsImageNotFound()
    {
        // Act
        var result = _converter.Convert(false, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal("Image Not Found", result);
    }

    [Fact]
    public void Convert_Null_ReturnsImageStatusNotChecked()
    {
        // Act
        var result = _converter.Convert(null, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal("Image status not checked", result);
    }

    [Fact]
    public void Convert_NonBool_ReturnsImageStatusNotChecked()
    {
        // Act
        var result = _converter.Convert("not a bool", typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal("Image status not checked", result);
    }

    [Fact]
    public void ConvertBack_ThrowsNotImplementedException()
    {
        // Act & Assert
        Assert.Throws<NotImplementedException>(() =>
            _converter.ConvertBack("Image Found", typeof(bool?), null, CultureInfo.InvariantCulture));
    }
}