using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Models.FormgenUtilities;
using AMFormsCST.Desktop.Models.FormgenUtilities.Grouping;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

namespace AMFormsCST.Test.Desktop.Models.FormgenUtilities;
public class CodeLineCollectionPropertiesTests
{
    private CodeLine MakeCodeLine(CodeType type, string? variable = null)
    {
        var settings = new AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings
        {
            Order = 0,
            Type = type,
            Variable = variable
        };
        return new CodeLine { Settings = settings };
    }
}
