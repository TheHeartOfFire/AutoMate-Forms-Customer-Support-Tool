using AMFormsCST.Desktop.Models.UserSettings;
using Xunit;
namespace AMFormsCST.Test.Desktop.Models.UserSettings;
public class NewTemplateSettingTests
{
    [Fact]
    public void Default_SelectNewTemplate_IsTrue()
    {
        // Act
        var setting = new NewTemplateSetting();

        // Assert
        Assert.True(setting.SelectNewTemplate);
    }

    [Fact]
    public void SelectNewTemplate_CanBeSetAndRetrieved()
    {
        // Arrange
        var setting = new NewTemplateSetting();

        // Act
        setting.SelectNewTemplate = false;

        // Assert
        Assert.False(setting.SelectNewTemplate);

        // Act
        setting.SelectNewTemplate = true;

        // Assert
        Assert.True(setting.SelectNewTemplate);
    }
}