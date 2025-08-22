using AMFormsCST.Core.Helpers;

namespace AMFormsCST.Core.Interfaces.Notebook;

public interface IForm : INotable<IForm>
{
    string Name { get; set; }
    bool Notable { get; set; }
    string Notes { get; set; }
    SelectableList<ITestDeal> TestDeals { get; set; }
}