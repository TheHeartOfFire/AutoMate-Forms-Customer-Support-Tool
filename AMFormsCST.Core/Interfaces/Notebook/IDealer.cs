using AMFormsCST.Core.Helpers;
using AMFormsCST.Core.Types.Notebook;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Interfaces.Notebook; 
[JsonDerivedType(typeof(Dealer), typeDiscriminator: "dealer")]
public interface IDealer : INotebookItem<IDealer>
{
    string Name { get; set; }
    string ServerCode { get; set; }
    SelectableList<ICompany> Companies { get; set; }
    bool Notable { get; set; }
}
