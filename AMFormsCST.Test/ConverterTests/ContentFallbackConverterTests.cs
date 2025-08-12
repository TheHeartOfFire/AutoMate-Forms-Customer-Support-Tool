using AMFormsCST.Desktop.Converters;
using AMFormsCST.Desktop.Models;
using AMFormsCST.Test.Helpers;
using System.Globalization;
using System.Windows;

namespace AMFormsCST.Test.ConverterTests;

public class ContentFallbackConverterTests
{
    private readonly ContentFallbackConverter _converter = new();

    [WpfFact]
    public void Convert_WithInvalidValues_ReturnsFallback()
    {
        // Arrange
        var values = new object[] { DependencyProperty.UnsetValue, "SomePath", "Fallback" };

        // Act
        var result = _converter.Convert(values, typeof(string), null, CultureInfo.CurrentCulture);

        // Assert
        Xunit.Assert.Equal("Fallback", result);
    }

    [Theory]
    [InlineData("SVR1", "Test Dealer", "(SVR1)Test Dealer")]
    [InlineData("", "Test Dealer", "()Test Dealer")]
    [InlineData("SVR1", "", "(SVR1)")]
    public void Convert_ForDealerWithCompositeName_ReturnsCorrectlyFormattedString(string serverCode, string dealerName, string expected)
    {
        // Arrange
        var dealer = new Dealer { ServerCode = serverCode, Name = dealerName };
        var values = new object[] { dealer, "CompositeDealerName", "Fallback" };

        // Act
        var result = _converter.Convert(values, typeof(string), null, CultureInfo.CurrentCulture);

        // Assert
        Xunit.Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_ForDealerWithEmptyCompositeName_ReturnsFallback()
    {
        // Arrange
        var dealer = new Dealer { ServerCode = "", Name = "" };
        var values = new object[] { dealer, "CompositeDealerName", "Fallback Dealer" };

        // Act
        var result = _converter.Convert(values, typeof(string), null, CultureInfo.CurrentCulture);

        // Assert
        Xunit.Assert.Equal("Fallback Dealer", result);
    }

    [Theory]
    [InlineData("C1", "Company One", "(C1)Company One")]
    [InlineData("", "Company One", "Company One")]
    [InlineData("C1", "", "(C1)")]
    public void Convert_ForCompanyWithCompositeName_ReturnsCorrectlyFormattedString(string companyCode, string companyName, string expected)
    {
        // Arrange
        var company = new Company { CompanyCode = companyCode, Name = companyName };
        var values = new object[] { company, "CompositeCompanyName", "Fallback" };

        // Act
        var result = _converter.Convert(values, typeof(string), null, CultureInfo.CurrentCulture);

        // Assert
        Xunit.Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_ForCompanyWithEmptyCompositeName_ReturnsFallback()
    {
        // Arrange
        var company = new Company { CompanyCode = "", Name = "" };
        var values = new object[] { company, "CompositeCompanyName", "Fallback Company" };

        // Act
        var result = _converter.Convert(values, typeof(string), null, CultureInfo.CurrentCulture);

        // Assert
        Xunit.Assert.Equal("Fallback Company", result);
    }

    [Fact]
    public void Convert_ForGenericProperty_WithValue_ReturnsValue()
    {
        // Arrange
        var dealer = new Dealer { Name = "Generic Dealer Name" };
        var values = new object[] { dealer, "Name", "Fallback" };

        // Act
        var result = _converter.Convert(values, typeof(string), null, CultureInfo.CurrentCulture);

        // Assert
        Xunit.Assert.Equal("Generic Dealer Name", result);
    }

    [Fact]
    public void Convert_ForGenericProperty_WithEmptyValue_ReturnsFallback()
    {
        // Arrange
        var dealer = new Dealer { Name = "" };
        var values = new object[] { dealer, "Name", "Fallback Name" };

        // Act
        var result = _converter.Convert(values, typeof(string), null, CultureInfo.CurrentCulture);

        // Assert
        Xunit.Assert.Equal("Fallback Name", result);
    }
}