using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models.FormgenUtilities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Shapes;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools;
public partial class TreeItemNodeViewModel : ViewModel
{
    [ObservableProperty]
    private string _nodeName = string.Empty;
    public ObservableCollection<TreeItemNodeViewModel> Children { get; set; } = [];

    [ObservableProperty]
    private IFormgenFileProperties _properties = new FormProperties();
    public enum Type { Form, Codelines, Init, Prompts, PostPrompts, Fields, Page, Tail }

    public void Initialize(DotFormgen formgenfile, Type type = Type.Form, string name = "", object? tail = null)
    {
        if (formgenfile is null) return;
        switch (type)
        {
            case Type.Form:
                NodeName = formgenfile.Title ?? string.Empty;

                var codelines = new TreeItemNodeViewModel();
                codelines.Initialize(formgenfile, Type.Codelines);

                var fields = new TreeItemNodeViewModel();
                fields.Initialize(formgenfile, Type.Fields);

                Children.Add(codelines);
                Children.Add(fields);

                Properties = new FormProperties
                {
                    Settings = new FormSettings
                    {
                        Version = formgenfile.Settings?.Version.ToString() ?? string.Empty,
                        PublishedUUID = formgenfile.Settings?.UUID ?? string.Empty,
                        LegacyImport = formgenfile.Settings?.LegacyImport ?? false,
                        TotalPages = formgenfile.Pages.Count,
                        DefaultPoints = formgenfile.Settings?.DefaultFontSize ?? 0,
                        MissingSourceJpeg = formgenfile.Settings?.MissingSourceJpeg ?? false,
                        Duplex = formgenfile.Settings?.Duplex ?? false,
                        MaxAccessoryLines = formgenfile.Settings?.MaxAccessoryLines ?? 0,
                        PrePrintedLaserForm = formgenfile.Settings?.PreprintedLaserForm ?? false
                    },
                    Title = formgenfile.Title ?? string.Empty,
                    TradePrompt = formgenfile.TradePrompt,
                    FormType = formgenfile.FormType,
                    BillingName = formgenfile.BillingName ?? string.Empty,
                    Category = formgenfile.Category,
                    SalesPersonPrompt = formgenfile.SalesPersonPrompt,
                    Username = formgenfile.Username ?? string.Empty
                };
                break;
            case Type.Codelines:
                NodeName = "Code Lines";

                var init = new TreeItemNodeViewModel();
                init.Initialize(formgenfile, Type.Init);

                var prompts = new TreeItemNodeViewModel();
                prompts.Initialize(formgenfile, Type.Prompts);

                var postPrompts = new TreeItemNodeViewModel();
                postPrompts.Initialize(formgenfile, Type.PostPrompts);

                Properties = new CodeLineStats{
                    Total = formgenfile.CodeLines.Count,
                    Init = formgenfile.CodeLines.Count(x => x.Settings?.Type == CodeType.INIT),
                    Prompts = formgenfile.CodeLines.Count(x => x.Settings?.Type == CodeType.PROMPT),
                    PostPrompts = formgenfile.CodeLines.Count(x => x.Settings?.Type == CodeType.POST)
                };

                Children.Add(init);
                Children.Add(prompts);
                Children.Add(postPrompts);
                break;
            case Type.Init:
                NodeName = "Init";

                foreach (var line in formgenfile.CodeLines.Where(x => x.Settings?.Type == CodeType.INIT))
                {
                    var item = new TreeItemNodeViewModel();
                    item.Initialize(formgenfile, Type.Tail, line.Expression ?? string.Empty, line);
                    Children.Add(item);
                }

                Properties = new BasicStats { Total = formgenfile.CodeLines.Count(x => x.Settings?.Type == CodeType.INIT) };
                break;
            case Type.Prompts:
                NodeName = "Prompts";

                foreach (var line in formgenfile.CodeLines.Where(x => x.Settings?.Type == CodeType.PROMPT))
                {
                    var item = new TreeItemNodeViewModel();

                    name = line.Settings?.Variable ?? string.Empty;
                    if(name.Equals("F0"))
                        name = line.PromptData?.Message ?? string.Empty;
                    if(line.PromptData?.Settings?.Type == Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings
                        .PromptType.Separator)
                        name = "~~Separator~~";

                    item.Initialize(formgenfile, Type.Tail, name, line);
                    Children.Add(item);
                }

                Properties = new BasicStats { Total = formgenfile.CodeLines.Count(x => x.Settings?.Type == CodeType.PROMPT) };
                break;
            case Type.PostPrompts:
                NodeName = "Post Prompts";

                foreach (var line in formgenfile.CodeLines.Where(x => x.Settings?.Type == CodeType.POST))
                {
                    var item = new TreeItemNodeViewModel();
                    item.Initialize(formgenfile, Type.Tail, line.Expression ?? string.Empty, line);
                    Children.Add(item);
                }

                Properties = new BasicStats { Total = formgenfile.CodeLines.Count(x => x.Settings?.Type == CodeType.POST) };
                break;
            case Type.Fields:
                NodeName = "Fields";

                foreach (var page in formgenfile.Pages)
                {
                    var item = new TreeItemNodeViewModel();
                    item.Initialize(formgenfile, Type.Page,
                        page.Settings?.PageNumber.ToString() ?? string.Empty);
                    Children.Add(item);
                }

                Properties = new FieldStats 
                { 
                    Fields = formgenfile.FieldCount(),
                    Pages = formgenfile.Pages.Count,
                };
                break;
            case Type.Page:
                if (!int.TryParse(name, out int pageNumber)) break;
                NodeName = "Page: " + name;

                var currentPage = formgenfile.Pages.FirstOrDefault(x => x.Settings?.PageNumber == pageNumber);

                if (currentPage is null) break;

                foreach (var field in currentPage.Fields)
                {
                    var item = new TreeItemNodeViewModel();
                    item.Initialize(formgenfile, Type.Tail, field.Expression ?? string.Empty, field);
                    Children.Add(item);
                }

                Properties = new PageProperties
                {
                    Total = currentPage.Fields.Count,
                    Settings = new PageSettings
                    {
                        PageNumber = currentPage.Settings?.PageNumber ?? 0,
                        BottomPrinterMargin = currentPage.Settings?.BottomPrinterMargin ?? 0,
                        DefaultFontSize = currentPage.Settings?.DefaultFontSize ?? 0,
                        LeftPrinterMargin = currentPage.Settings?.LeftPrinterMargin ?? 0,
                        RightPrinterMargin = currentPage.Settings?.RightPrinterMargin ?? 0,
                        TopPrinterMargin = currentPage.Settings?.TopPrinterMargin ?? 0
                    }
                };
                break;
            case Type.Tail:
                NodeName = name;
                if (tail is CodeLine codeLine)
                {
                    Properties = new CodeLineProperties
                    {
                        Expression = codeLine.Expression ?? string.Empty,
                        Settings = new Models.FormgenUtilities.CodeLineSettings
                        {
                            Order = codeLine.Settings?.Order ?? 0,
                            Type = codeLine.Settings?.Type ?? CodeType.PROMPT,
                            Variable = codeLine.Settings?.Variable
                        },
                        PromptData = new PromptDataProperties
                        {
                            Message = codeLine.PromptData?.Message ?? string.Empty,
                            Settings = new Models.FormgenUtilities.PromptDataSettings
                            {
                                Type = codeLine.PromptData?.Settings?.Type ?? Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings.PromptType.Text,
                                AllowNegative = codeLine.PromptData?.Settings?.AllowNegative ?? false,
                                IncludeNoneAsOption = codeLine.PromptData?.Settings?.IncludeNoneAsOption ?? false,
                                DecimalPlaces = codeLine.PromptData?.Settings?.DecimalPlaces ?? 0,
                                Delimiter = codeLine.PromptData?.Settings?.Delimiter ?? string.Empty,
                                ForceUpperCase = codeLine.PromptData?.Settings?.ForceUpperCase ?? false,
                                IsExpression = codeLine.PromptData?.Settings?.IsExpression ?? false,
                                Length = codeLine.PromptData?.Settings?.Length ?? 0,
                                MakeBuyerVars = codeLine.PromptData?.Settings?.MakeBuyerVars ?? false,
                                Required = codeLine.PromptData?.Settings?.Required ?? false
                            },
                            Choices = codeLine.PromptData?.Choices ?? []
                        }
                    };
                }
                else if (tail is FormField field)
                {
                    Properties = new FieldProperties
                    {
                        Expression = field.Expression ?? string.Empty,
                        Settings = new FieldSettings
                        {
                            FontAlignment = field.Settings?.FontAlignment ?? FormFieldSettings.Alignment.LEFT,
                            Bold = field.Settings?.Bold ?? false,
                            DecimalPlaces = field.Settings?.DecimalPlaces ?? 0,
                            DisplayPartial = field.Settings?.DisplayPartial ?? false,
                            EndIndex = field.Settings?.EndIndex ?? 0,
                            FontSize = field.Settings?.FontSize ?? 0,
                            ID = field.Settings?.ID ?? 0,
                            ImpactPosition = field.Settings?.ImpactPosition ?? new System.Drawing.Point(0, 0), 
                            Kerning = field.Settings?.Kearning ?? 0, 
                            LaserRect = field.Settings?.LaserRect ?? new System.Drawing.Rectangle(),
                            Length = field.Settings?.Length ?? 0, 
                            ManualSize = field.Settings?.ManualSize ?? false, 
                            ShrinkToFit = field.Settings?.ShrinkToFit ?? false, 
                            StartIndex = field.Settings?.StartIndex ?? 0 
                        },
                        FormattingOption = field.FormattingOption,
                        SampleData = field.SampleData ?? string.Empty
                    };
                }
                break;

        }
    }

}
