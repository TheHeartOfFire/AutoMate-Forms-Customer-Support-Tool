using AMFormsCST.Core.Types.Notebook;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Interfaces.Notebook;
[JsonDerivedType(typeof(TestDeal), typeDiscriminator: "testdeal")]

public interface ITestDeal : INotebookItem<ITestDeal>
{
    string DealNumber { get; set; }
    string Purpose { get; set; }
}