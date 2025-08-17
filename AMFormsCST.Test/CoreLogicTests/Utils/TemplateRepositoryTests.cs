using AMFormsCST.Core;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Core.Utils;
using System.Collections.Generic;
using Xunit;

public class TemplateRepositoryTests
{
    [Fact]
    public void LoadTemplates_ReturnsList()
    {
        // Arrange
        var repo = new TemplateRepository();

        // Act
        var result = repo.LoadTemplates();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<TextTemplate>>(result);
    }

    [Fact]
    public void SaveTemplates_DoesNotThrow()
    {
        // Arrange
        var repo = new TemplateRepository();
        var templates = new List<TextTemplate>
        {
            new TextTemplate("T1", "D1", "Text1")
        };

        // Act & Assert
        var ex = Record.Exception(() => repo.SaveTemplates(templates));
        Assert.Null(ex);
    }
}