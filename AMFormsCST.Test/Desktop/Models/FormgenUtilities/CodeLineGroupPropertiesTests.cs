using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using AMFormsCST.Desktop.Models.FormgenUtilities;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using CodeLineSettings = AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

namespace AMFormsCST.Test.Desktop.Models.FormgenUtilities;
public class CodeLineGroupPropertiesTests
{
    private CodeLine MakeCodeLine(CodeType type, string? variable = null)
    {
        var settings = new CodeLineSettings
        {
            Type = type,
            Variable = variable
        };
        return new CodeLine { Settings = settings };
    }
}