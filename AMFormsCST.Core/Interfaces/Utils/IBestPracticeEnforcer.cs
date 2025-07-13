using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;

namespace AMFormsCST.Core.Interfaces.Utils;
public interface IBestPracticeEnforcer
{
    IFormNameBestPractice FormNameBestPractice { get; }
    List<TextTemplate> Templates { get; }

    string GetFormName();
}