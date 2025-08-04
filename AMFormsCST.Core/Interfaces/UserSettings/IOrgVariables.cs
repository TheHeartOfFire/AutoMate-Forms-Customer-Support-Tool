using AMFormsCST.Core.Interfaces.BestPractices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Interfaces.UserSettings;
public interface IOrgVariables : ISetting
{
    Dictionary<string, string> LooseVariables { get; set; }
    List<ITextTemplateVariable> Variables { get; }
}
