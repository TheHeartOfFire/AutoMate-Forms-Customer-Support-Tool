using AMFormsCST.Desktop.Models;

namespace AMFormsCST.Test.Desktop.Models.Notebook;
using Assert = Assert;

public class CompanyModelTests
{
    [Fact]
    public void IsBlank_IsTrue_ForNewModel()
    {
        // Arrange
        var company = new Company();

        // Assert
        Assert.True(company.IsBlank);
    }

    [Theory]
    [InlineData("Test Company", null)]
    [InlineData(null, "C123")]
    public void IsBlank_IsFalse_WhenPropertiesAreSet(string? name, string? companyCode)
    {
        // Arrange
        var company = new Company();

        // Act
        company.Name = name;
        company.CompanyCode = companyCode;

        // Assert
        Assert.False(company.IsBlank);
    }

    [Fact]
    public void SelectAndDeselect_UpdateIsSelectedProperty()
    {
        // Arrange
        var company = new Company();
        Assert.False(company.IsSelected); // Initial state

        // Act: Select
        company.Select();

        // Assert
        Assert.True(company.IsSelected);

        // Act: Deselect
        company.Deselect();

        // Assert
        Assert.False(company.IsSelected);
    }
}