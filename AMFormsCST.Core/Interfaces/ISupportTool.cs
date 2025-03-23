using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.Utils;

namespace AMFormsCST.Core.Interfaces;
public interface ISupportTool
{
    ICodeBlocks CodeBlocks { get; set; }
    IBestPracticeEnforcer Enforcer { get; set; }
    IFormgenUtils FormgenUtils { get; set; }
    INotebook Notebook { get; set; }
    IReadOnlyCollection<ITextTemplateVariable> Variables { get; }
}