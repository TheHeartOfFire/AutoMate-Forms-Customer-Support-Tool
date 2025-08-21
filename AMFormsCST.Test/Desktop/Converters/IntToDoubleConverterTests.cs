using AMFormsCST.Desktop.Converters;
using System;
using System.Globalization;
using Xunit;

namespace AMFormsCST.Test.Desktop.Converters;

public class IntToDoubleConverterTests
{
    private readonly IntToDoubleConverter _converter = new();

    [Fact]
    public void Convert_Int_ReturnsDouble()
    {
        // Act
        var result = _converter.Convert(42, typeof(double), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<double>(result);
        Assert.Equal(42d, result);
    }

    [Fact]
    public void Convert_Double_ReturnsSameDouble()
    {
        // Act
        var result = _converter.Convert(3.14, typeof(double), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<double>(result);
        Assert.Equal(3.14, result);
    }

    [Fact]
    public void Convert_OtherType_ReturnsZeroDouble()
    {
        // Act
        var result = _converter.Convert("not a number", typeof(double), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<double>(result);
        Assert.Equal(0d, result);
    }

    [Fact]
    public void ConvertBack_Double_ReturnsInt()
    {
        // Act
        var result = _converter.ConvertBack(99.99, typeof(int), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<int>(result);
        Assert.Equal(99, result);
    }

    [Fact]
    public void ConvertBack_Int_ReturnsSameInt()
    {
        // Act
        var result = _converter.ConvertBack(123, typeof(int), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<int>(result);
        Assert.Equal(123, result);
    }

    [Fact]
    public void ConvertBack_OtherType_ReturnsZeroInt()
    {
        // Act
        var result = _converter.ConvertBack("not a number", typeof(int), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsType<int>(result);
        Assert.Equal(0, result);
    }
}