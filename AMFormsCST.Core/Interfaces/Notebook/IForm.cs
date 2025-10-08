using AMFormsCST.Core.Helpers;
using AMFormsCST.Core.Types.Notebook;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Interfaces.Notebook;
[JsonDerivedType(typeof(Form), typeDiscriminator: "form")]

public interface IForm : INotebookItem<IForm>
{
    string Name { get; set; }
    bool Notable { get; set; }
    string Notes { get; set; }
    FormFormat Format { get; set; }
    SelectableList<ITestDeal> TestDeals { get; set; }

    public enum FormFormat
    {
        LegacyImpact,
        Pdf
    }
}