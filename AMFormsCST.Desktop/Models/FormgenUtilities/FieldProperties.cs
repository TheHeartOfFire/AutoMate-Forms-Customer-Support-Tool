using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.FormField;

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

    public FormatOption FormattingOption
    {
        get => _coreField.FormattingOption;
        set => SetProperty(_coreField.FormattingOption, value, _coreField, (f, v) => f.FormattingOption = v);
    }

    public FieldProperties(FormField field)
    {
        _coreField = field;
        Settings = new FieldSettings(field.Settings);
    }

    public UIElement GetUIElements()
    {
        return BasicStats.GetSettingsAndPropertiesUIElements(this);
    }
}
