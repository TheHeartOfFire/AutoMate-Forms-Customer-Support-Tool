using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Windows;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public partial class FieldProperties : ObservableObject, IFormgenFileProperties
{
    private readonly FormField _coreField;

    public IFormgenFileSettings Settings { get; set; }

    public string? Expression
    {
        get => _coreField.Expression;
        set => SetProperty(_coreField.Expression, value, _coreField, (f, v) => f.Expression = v);
    }

    public string? SampleData
    {
        get => _coreField.SampleData;
        set => SetProperty(_coreField.SampleData, value, _coreField, (f, v) => f.SampleData = v);
    }

    public FormField.FormatOption FormattingOption
    {
        get => _coreField.FormattingOption;
        set => SetProperty(_coreField.FormattingOption, value, _coreField, (f, v) => f.FormattingOption = v);
    }

    public FieldProperties(FormField field)
    {
        _coreField = field;
        Settings = new FieldSettings(field.Settings);
    }

    public IEnumerable<DisplayProperty> GetDisplayProperties()
    {
        yield return new DisplayProperty("Expression:", Expression);
        yield return new DisplayProperty("Sample Data:", SampleData);
        yield return new DisplayProperty("Formatting:", FormattingOption.ToString());

        if (Settings is FieldSettings fieldSettings)
        {
            yield return new DisplayProperty("ID:", fieldSettings.ID.ToString());
            yield return new DisplayProperty("Type:", fieldSettings.GetFieldType());
            yield return new DisplayProperty("Font Size:", fieldSettings.FontSize.ToString());
            yield return new DisplayProperty("Alignment:", fieldSettings.GetFontAlignment());
            yield return new DisplayProperty("Bold:", fieldSettings.Bold.ToString());
            yield return new DisplayProperty("Shrink to Fit:", fieldSettings.ShrinkToFit.ToString());
        }
    }
}
