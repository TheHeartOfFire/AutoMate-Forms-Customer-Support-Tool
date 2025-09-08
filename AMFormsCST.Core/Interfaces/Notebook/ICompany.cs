using AMFormsCST.Core.Types.Notebook;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Interfaces.Notebook;
[JsonDerivedType(typeof(Company), typeDiscriminator: "company")]
public interface ICompany : INotebookItem<ICompany>
{
    string Name { get; set; }
    string CompanyCode { get; set; }
    bool Notable { get; set; }
}
