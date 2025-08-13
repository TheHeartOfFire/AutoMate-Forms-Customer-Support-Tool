using AMFormsCST.Desktop.Converters;
using System;
using System.Globalization;
using System.Windows.Data;
using Xunit;

public class EnumToBooleanConverterTests
{
    private enum TestEnum { First, Second, Third }

    private readonly EnumToBooleanConverter _converter = new();

    [Fact]
    public void Convert_ReturnsTrue_WhenEnumMatchesParameter_CaseInsensitive()
    {
        // Arrange
        var value = TestEnum.Second;
        var parameter = "second";

        // Act
        var result = _converter.Convert(value, typeof(bool), parameter, CultureInfo.InvariantCulture);

        // Assert
        Assert.True((bool)result);
    }

    [Fact]
    public void Convert_ReturnsFalse_WhenEnumDoesNotMatchParameter()
    {
        // Arrange
        var value = TestEnum.First;
        var parameter = "third";

        // Act
        var result = _converter.Convert(value, typeof(bool), parameter, CultureInfo.InvariantCulture);

        // Assert
        Assert.False((bool)result);
    }

    [Fact]
    public void Convert_ReturnsFalse_WhenValueIsNull()
    {
        // Arrange
        object value = null;
        var parameter = "First";

        // Act
        var result = _converter.Convert(value, typeof(bool), parameter, CultureInfo.InvariantCulture);

        // Assert
        Assert.False((bool)result);
    }

    [Fact]
    public void Convert_ReturnsFalse_WhenParameterIsNull()
    {
        // Arrange
        var value = TestEnum.First;
        object parameter = null;

        // Act
        var result = _converter.Convert(value, typeof(bool), parameter, CultureInfo.InvariantCulture);

        // Assert
        Assert.False((bool)result);
    }

    [Fact]
    public void ConvertBack_ReturnsEnum_WhenValueIsTrue()
    {
        // Arrange
        var value = true;
        var parameter = "Second";

        // Act
        var result = _converter.ConvertBack(value, typeof(TestEnum), parameter, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(TestEnum.Second, result);
    }

    [Fact]
    public void ConvertBack_ReturnsBindingDoNothing_WhenValueIsFalse()
    {
        // Arrange
        var value = false;
        var parameter = "First";

        // Act
        var result = _converter.ConvertBack(value, typeof(TestEnum), parameter, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(Binding.DoNothing, result);
    }

    [Fact]
    public void ConvertBack_ReturnsBindingDoNothing_WhenParameterIsNull()
    {
        // Arrange
        var value = true;
        object parameter = null;

        // Act
        var result = _converter.ConvertBack(value, typeof(TestEnum), parameter, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(Binding.DoNothing, result);
    }
}