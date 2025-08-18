using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using Moq;
using System.Collections.Generic;
using Xunit;

public class TemplateRepositoryTests
{
    [Fact]
    public void LoadTemplates_ReturnsList()
    {
        // Arrange
        var mockRepo = new Mock<ITemplateRepository>();
        var expectedList = new List<TextTemplate>
        {
            new TextTemplate("T1", "D1", "Text1", TextTemplate.TemplateType.Other)
        };
        mockRepo.Setup(r => r.LoadTemplates()).Returns(expectedList);

        // Act
        var result = mockRepo.Object.LoadTemplates();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<TextTemplate>>(result);
        Assert.Same(expectedList, result);
    }

    [Fact]
    public void SaveTemplates_DoesNotThrow()
    {
        var mockRepo = new Mock<ITemplateRepository>();
        mockRepo.Setup(r => r.SaveTemplates(It.IsAny<List<TextTemplate>>()));

        var templates = new List<TextTemplate>
        {
            new TextTemplate("T1", "D1", "Text1", TextTemplate.TemplateType.Other)
        };

        var ex = Record.Exception(() => mockRepo.Object.SaveTemplates(templates));
        Assert.Null(ex);
    }
}