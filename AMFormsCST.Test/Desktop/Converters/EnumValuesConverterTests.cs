using AMFormsCST.Desktop.Converters;
using System;
using System.Globalization;
using Xunit;

namespace AMFormsCST.Test.Desktop.Converters;

public class EnumValuesConverterTests
{
    private enum TestEnum
    {
        First,
        Second,
        Third
    }

    [Fact]
    public void Convert_WithEnumType_ReturnsEnumValues()
    {
        // Arrange
        var converter = new EnumValuesConverter();

        // Act
        var result = converter.Convert(typeof(TestEnum), typeof(object), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<Array>(result); // Accepts any array type
        var values = Assert.IsType<TestEnum[]>(result); // Checks for the specific enum array type
        Assert.Equal([TestEnum.First, TestEnum.Second, TestEnum.Third], values);
    }

    [Fact]
    public void Convert_WithNonEnumType_ReturnsEmptyArray()
    {
        // Arrange
        var converter = new EnumValuesConverter();

        // Act
        var result = converter.Convert(typeof(string), typeof(object), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.NotNull(result);
        var values = Assert.IsType<object[]>(result);
        Assert.Empty(values);
    }

    [Fact]
    public void Convert_WithNonTypeObject_ReturnsEmptyArray()
    {
        // Arrange
        var converter = new EnumValuesConverter();

        // Act
        var result = converter.Convert(123, typeof(object), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.NotNull(result);
        var values = Assert.IsType<object[]>(result);
        Assert.Empty(values);
    }

    [Fact]
    public void ConvertBack_Always_ReturnsBindingDoNothing()
    {
        // Arrange
        var converter = new EnumValuesConverter();

        // Act
        var result = converter.ConvertBack(TestEnum.First, typeof(object), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(System.Windows.Data.Binding.DoNothing, result);
    }
}