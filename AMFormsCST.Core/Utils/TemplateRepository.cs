using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using System.Collections.Generic;

namespace AMFormsCST.Core.Utils;

public class TemplateRepository : ITemplateRepository
{
    public List<TextTemplate> LoadTemplates() => IO.LoadTemplates();

    public void SaveTemplates(List<TextTemplate> templates) => IO.SaveTemplates(templates);
}