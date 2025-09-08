using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Core.Types.BestPractices.TextTemplates.Models;

public class TextTemplateVariableTests
{
    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        // Arrange
        var properName = "{TestProperName}";
        var name = "TestName";
        var prefix = "$";
        var description = "A test variable.";
        var aliases = new List<string> { "t1", "t2" };
        var getValueFunc = new Func<string>(() => "TestValue");

        // Act
        var variable = new TextTemplateVariable(properName, name, prefix, description, aliases, getValueFunc);

        // Assert
        Assert.Equal(properName, variable.ProperName);
        Assert.Equal(name, variable.Name);
        Assert.Equal(prefix, variable.Prefix);
        Assert.Equal(description, variable.Description);
        Assert.Equal(aliases, variable.Aliases.ToList()); // Compare contents
    }

    [Fact]
    public void GetValue_InvokesAndReturnsCorrectValueFromFunc()
    {
        // Arrange
        var expectedValue = "The result of the function";
        var getValueFunc = new Func<string>(() => expectedValue);
        var variable = new TextTemplateVariable("p", "n", "$", "d", [], getValueFunc);

        // Act
        var result = variable.GetValue();

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Fact]
    public void Aliases_ReturnsReadOnlyCollection()
    {
        // Arrange
        var aliases = new List<string> { "a1", "a2" };
        var variable = new TextTemplateVariable("p", "n", "$", "d", aliases, () => "");

        // Act
        var resultAliases = variable.Aliases;

        // Assert
        Assert.IsAssignableFrom<IReadOnlyCollection<string>>(resultAliases);
        Assert.Equal(2, resultAliases.Count);
        Assert.Contains("a1", resultAliases);
        Assert.Contains("a2", resultAliases);
    }
}