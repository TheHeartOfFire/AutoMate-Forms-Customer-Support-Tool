using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using System.Collections.Generic;

namespace AMFormsCST.Core.Interfaces.Utils;

public interface ITemplateRepository
{
    List<TextTemplate> LoadTemplates();
    void SaveTemplates(List<TextTemplate> templates);
}