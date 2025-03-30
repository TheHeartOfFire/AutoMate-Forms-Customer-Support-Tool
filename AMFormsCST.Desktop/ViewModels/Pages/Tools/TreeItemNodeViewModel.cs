using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using AMFormsCST.Desktop.Controls.FormgenUtilities;
using AMFormsCST.Desktop.Interfaces;
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

    public void Initialize(DotFormgen formgenfile, Type type = Type.Form, string name = "")
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
                    Version = formgenfile.Settings?.Version.ToString() ?? string.Empty,
                    PublishedUUID = formgenfile.Settings?.UUID ?? string.Empty,
                    LegacyImport = formgenfile.Settings?.LegacyImport ?? false,
                    TotalPages = formgenfile.Pages.Count,
                    DefaultPoints = formgenfile.Settings?.DefaultFontSize ?? 0,
                    MissingSourceJpeg = formgenfile.Settings?.MissingSourceJpeg ?? false,
                    Duplex = formgenfile.Settings?.Duplex ?? false,
                    MaxAccessoryLines = formgenfile.Settings?.MaxAccessoryLines ?? 0,
                    PrePrintedLaserForm = formgenfile.Settings?.PreprintedLaserForm ?? false
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

                Children.Add(init);
                Children.Add(prompts);
                Children.Add(postPrompts);
                break;
            case Type.Init:
                NodeName = "Init";

                foreach (var line in formgenfile.CodeLines.Where(x => x.Settings?.Type == CodeType.INIT))
                {
                    var item = new TreeItemNodeViewModel();
                    item.Initialize(formgenfile, Type.Tail, line.Expression ?? string.Empty);
                    Children.Add(item);
                }

                break;
            case Type.Prompts:
                NodeName = "Prompts";

                foreach (var line in formgenfile.CodeLines.Where(x => x.Settings?.Type == CodeType.PROMPT))
                {
                    var item = new TreeItemNodeViewModel();

                    name = line.Settings?.Variable ?? string.Empty;
                    if(name.Equals("F0"))
                        name = line.PromptData?.Message ?? string.Empty;
                    if(line.PromptData?.Settings?.Type == PromptDataSettings.PromptType.Separator)
                        name = "~~Separator~~";

                    item.Initialize(formgenfile, Type.Tail, name);
                    Children.Add(item);
                }

                break;
            case Type.PostPrompts:
                NodeName = "Post Prompts";

                foreach (var line in formgenfile.CodeLines.Where(x => x.Settings?.Type == CodeType.POST))
                {
                    var item = new TreeItemNodeViewModel();
                    item.Initialize(formgenfile, Type.Tail, line.Expression ?? string.Empty);
                    Children.Add(item);
                }

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

                break;
            case Type.Page:
                if (!int.TryParse(name, out int pageNumber)) break;
                NodeName = "Page: " + name;

                var currentPage = formgenfile.Pages.FirstOrDefault(x => x.Settings?.PageNumber == pageNumber);

                if (currentPage is null) break;

                foreach (var field in currentPage.Fields)
                {
                    var item = new TreeItemNodeViewModel();
                    item.Initialize(formgenfile, Type.Tail,field.Expression ?? string.Empty);
                    Children.Add(item);
                }

                break;
            case Type.Tail:
                NodeName = name;

                break;
        }
    }

}
