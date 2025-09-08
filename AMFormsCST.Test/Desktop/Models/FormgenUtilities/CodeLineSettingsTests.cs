using AMFormsCST.Desktop.Models.FormgenUtilities;
using CoreSettings = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;
using CoreCodeType = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings.CodeType;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Desktop.Models.FormgenUtilities;

public class CodeLineSettingsTests
{
    [Fact]
    public void Order_SetProperty_UpdatesCoreModel()
    {
        // Arrange
        var coreSettings = new CoreSettings { Order = 1 };
        var wrapper = new CodeLineSettings(coreSettings);

        // Act
        wrapper.Order = 99;

        // Assert
        Assert.Equal(99, coreSettings.Order);
    }

    [Fact]
    public void Type_SetProperty_UpdatesCoreModel()
    {
        // Arrange
        var coreSettings = new CoreSettings { Type = CoreCodeType.INIT };
        var wrapper = new CodeLineSettings(coreSettings);

        // Act
        wrapper.Type = CoreCodeType.POST;

        // Assert
        Assert.Equal(CoreCodeType.POST, coreSettings.Type);
    }

    [Fact]
    public void Variable_SetProperty_UpdatesCoreModel()
    {
        // Arrange
        var coreSettings = new CoreSettings { Variable = "OldVar" };
        var wrapper = new CodeLineSettings(coreSettings);

        // Act
        wrapper.Variable = "NewVar";

        // Assert
        Assert.Equal("NewVar", coreSettings.Variable);
    }

    [Theory]
    [InlineData(CoreCodeType.INIT, "INIT")]
    [InlineData(CoreCodeType.PROMPT, "PROMPT")]
    [InlineData(CoreCodeType.POST, "POST")]
    [InlineData((CoreCodeType)99, "PROMPT")] // Test default case
    public void GetCodeType_ReturnsCorrectString_ForEnumValue(CoreCodeType type, string expected)
    {
        // Arrange
        var coreSettings = new CoreSettings { Type = type };
        var wrapper = new CodeLineSettings(coreSettings);

        // Act
        var result = wrapper.GetCodeType();

        // Assert
        Assert.Equal(expected, result);
    }
}