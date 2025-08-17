using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Xunit;
using AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

namespace AMFormsCST.Test.CoreLogicTests.FormgenFileStructure;

public class PromptDataTests
{
    // Always alias the live sample data provider
    private static readonly IEnumerable<object[]> FormgenFilePaths = FormgenTestDataHelper.FormgenFilePaths;

    [Theory]
    [MemberData(nameof(FormgenFilePaths), MemberType = typeof(FormgenTestDataHelper))]
    public void XmlConstructor_ParsesPromptDataFromSampleFiles(string formgenFilePath)
    {
        var dotFormgen = FormgenTestDataHelper.LoadDotFormgen(formgenFilePath);

        foreach (var codeLine in dotFormgen.CodeLines)
        {
            // Only test prompt types
            if (codeLine.PromptData != null)
            {
                var promptData = codeLine.PromptData;
                Assert.NotNull(promptData.Choices);
                // Choices may be empty, but should not be null
            }
        }
    }
}