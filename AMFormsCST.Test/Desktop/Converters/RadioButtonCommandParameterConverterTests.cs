using AMFormsCST.Desktop.Converters;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using Xunit;

namespace AMFormsCST.Test.Desktop.Converters;
public class RadioButtonCommandParameterConverterTests
{
    private readonly RadioButtonCommandParameterConverter _converter = new();

    private class TestItem
    {
        public string Id { get; set; } = "TestId";
        public int Number { get; set; } = 42;
    }

    [Fact]
    public void Convert_ReturnsNull_WhenValuesIsNullOrTooShort()
    {
        // Arrange
        object[] values = null;

        // Act
        var result = _converter.Convert(values, typeof(object), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Null(result);

        // Arrange
        values = new object[] { new TestItem() };

        // Act
        result = _converter.Convert(values, typeof(object), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Convert_ReturnsNull_WhenUnsetValuePresent()
    {
        // Arrange
        var values = new object[] { DependencyProperty.UnsetValue, "Id" };

        // Act
        var result = _converter.Convert(values, typeof(object), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Null(result);

        values = new object[] { new TestItem(), DependencyProperty.UnsetValue };

        result = _converter.Convert(values, typeof(object), null, CultureInfo.InvariantCulture);

        Assert.Null(result);
    }

    [Fact]
    public void Convert_ReturnsNull_WhenDataItemOrPathIsNullOrEmpty()
    {
        // Arrange
        var values = new object[] { null, "Id" };

        // Act
        var result = _converter.Convert(values, typeof(object), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Null(result);

        values = new object[] { new TestItem(), null };

        result = _converter.Convert(values, typeof(object), null, CultureInfo.InvariantCulture);

        Assert.Null(result);

        values = new object[] { new TestItem(), "" };

        result = _converter.Convert(values, typeof(object), null, CultureInfo.InvariantCulture);

        Assert.Null(result);
    }

    [Fact]
    public void Convert_ReturnsDataItem_WhenPathIsDataContext()
    {
        // Arrange
        var item = new TestItem();
        var values = new object[] { item, "DataContext" };

        // Act
        var result = _converter.Convert(values, typeof(object), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Same(item, result);
    }

    [Fact]
    public void Convert_ReturnsPropertyValue_WhenPathIsPropertyName()
    {
        // Arrange
        var item = new TestItem { Id = "ABC", Number = 99 };
        var values = new object[] { item, "Id" };

        // Act
        var result = _converter.Convert(values, typeof(object), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal("ABC", result);

        // Test with another property
        values = new object[] { item, "Number" };
        result = _converter.Convert(values, typeof(object), null, CultureInfo.InvariantCulture);
        Assert.Equal(99, result);
    }

    [Fact]
    public void Convert_ReturnsNull_WhenPropertyDoesNotExist()
    {
        // Arrange
        var item = new TestItem();
        var values = new object[] { item, "NonExistentProperty" };

        // Act
        var result = _converter.Convert(values, typeof(object), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertBack_ThrowsNotImplementedException()
    {
        // Act & Assert
        Assert.Throws<NotImplementedException>(() =>
            _converter.ConvertBack("value", new Type[] { typeof(object), typeof(string) }, null, CultureInfo.InvariantCulture));
    }
}