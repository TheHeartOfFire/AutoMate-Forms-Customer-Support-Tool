using AMFormsCST.Desktop.Converters;
using System;
using System.Globalization;
using Wpf.Ui.Controls;
using Xunit;

public class NullableBoolToSymbolConverterTests
{
    private readonly NullableBoolToSymbolConverter _converter = new();

    [Fact]
    public void Convert_True_ReturnsCheckmark24()
    {
        // Act
        var result = _converter.Convert(true, typeof(SymbolRegular), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(SymbolRegular.Checkmark24, result);
    }

    [Fact]
    public void Convert_False_ReturnsDismiss24()
    {
        // Act
        var result = _converter.Convert(false, typeof(SymbolRegular), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(SymbolRegular.Dismiss24, result);
    }

    [Fact]
    public void Convert_Null_ReturnsQuestion24()
    {
        // Act
        var result = _converter.Convert(null, typeof(SymbolRegular), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(SymbolRegular.Question24, result);
    }

    [Fact]
    public void Convert_NonBool_ReturnsQuestion24()
    {
        // Act
        var result = _converter.Convert("not a bool", typeof(SymbolRegular), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(SymbolRegular.Question24, result);
    }

    [Fact]
    public void ConvertBack_ThrowsNotImplementedException()
    {
        // Act & Assert
        Assert.Throws<NotImplementedException>(() =>
            _converter.ConvertBack(SymbolRegular.Checkmark24, typeof(bool?), null, CultureInfo.InvariantCulture));
    }
}