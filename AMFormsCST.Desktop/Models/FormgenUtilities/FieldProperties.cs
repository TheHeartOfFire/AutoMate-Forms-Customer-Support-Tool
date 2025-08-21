using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
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
        // Editable properties from FormField
        var fieldType = typeof(FormField);
        var exprProp = fieldType.GetProperty(nameof(FormField.Expression));
        if (exprProp != null)
            yield return new DisplayProperty(_coreField, exprProp);

        var sampleDataProp = fieldType.GetProperty(nameof(FormField.SampleData));
        if (sampleDataProp != null)
            yield return new DisplayProperty(_coreField, sampleDataProp);

        var formatOptionProp = fieldType.GetProperty(nameof(FormField.FormattingOption));
        if (formatOptionProp != null)
            yield return new DisplayProperty(_coreField, formatOptionProp);

        // Editable properties from FieldSettings
        if (Settings is FieldSettings fieldSettings)
        {
            var settingsType = typeof(FieldSettings);

            var idProp = settingsType.GetProperty(nameof(FieldSettings.ID));
            if (idProp != null)
                yield return new DisplayProperty(fieldSettings, idProp, true); // ID is usually read-only

            var typeProp = settingsType.GetProperty(nameof(FieldSettings.Type));
            if (typeProp != null)
                yield return new DisplayProperty(fieldSettings, typeProp);

            var fontSizeProp = settingsType.GetProperty(nameof(FieldSettings.FontSize));
            if (fontSizeProp != null)
                yield return new DisplayProperty(fieldSettings, fontSizeProp);

            var fontAlignmentProp = settingsType.GetProperty(nameof(FieldSettings.FontAlignment));
            if (fontAlignmentProp != null)
                yield return new DisplayProperty(fieldSettings, fontAlignmentProp);

            var boldProp = settingsType.GetProperty(nameof(FieldSettings.Bold));
            if (boldProp != null)
                yield return new DisplayProperty(fieldSettings, boldProp);

            var shrinkToFitProp = settingsType.GetProperty(nameof(FieldSettings.ShrinkToFit));
            if (shrinkToFitProp != null)
                yield return new DisplayProperty(fieldSettings, shrinkToFitProp);
        }
    }
}
