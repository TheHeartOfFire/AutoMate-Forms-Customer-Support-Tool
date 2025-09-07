using AMFormsCST.Core.Types.Notebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Interfaces.Notebook;
[JsonDerivedType(typeof(Company), typeDiscriminator: "company")]
public interface ICompany : INotebookItem<ICompany>
{
    string Name { get; set; }
    string CompanyCode { get; set; }
    bool Notable { get; set; }
}
