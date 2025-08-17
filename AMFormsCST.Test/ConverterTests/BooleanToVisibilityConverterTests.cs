using AMFormsCST.Desktop.Converters;
using System;
using System.Globalization;
using System.Windows;
using Xunit;

public class BooleanToVisibilityConverterTests
{
    private readonly BooleanToVisibilityConverter _converter = new();

    [Fact]
    public void Convert_True_ReturnsVisible()
    {
        // Act
        var result = _converter.Convert(true, typeof(Visibility), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(Visibility.Visible, result);
    }

    [Fact]
    public void Convert_False_ReturnsCollapsedByDefault()
    {
        // Act
        var result = _converter.Convert(false, typeof(Visibility), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(Visibility.Collapsed, result);
    }

    [Fact]
    public void Convert_Null_ReturnsCollapsedByDefault()
    {
        // Act
        var result = _converter.Convert(null, typeof(Visibility), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(Visibility.Collapsed, result);
    }

    [Fact]
    public void Convert_False_WithCustomFalseVisibility_ReturnsHidden()
    {
        // Arrange
        _converter.FalseVisibility = Visibility.Hidden;

        // Act
        var result = _converter.Convert(false, typeof(Visibility), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(Visibility.Hidden, result);
    }

    [Fact]
    public void Convert_True_WithInvert_ReturnsCollapsed()
    {
        // Arrange
        _converter.Invert = true;

        // Act
        var result = _converter.Convert(true, typeof(Visibility), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(Visibility.Collapsed, result);

        // Reset for other tests
        _converter.Invert = false;
    }

    [Fact]
    public void Convert_False_WithInvert_ReturnsVisible()
    {
        // Arrange
        _converter.Invert = true;

        // Act
        var result = _converter.Convert(false, typeof(Visibility), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(Visibility.Visible, result);

        // Reset for other tests
        _converter.Invert = false;
    }

    [Fact]
    public void ConvertBack_ThrowsNotSupportedException()
    {
        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            _converter.ConvertBack(Visibility.Visible, typeof(bool), null, CultureInfo.InvariantCulture));
    }
}