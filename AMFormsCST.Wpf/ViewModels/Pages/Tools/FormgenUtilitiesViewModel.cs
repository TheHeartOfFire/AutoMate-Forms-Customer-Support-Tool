using AMFormsCST.Core.Interfaces.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools;

public partial class FormgenUtilitiesViewModel : ViewModel
{
    [ObservableProperty]
    private IFormgenUtils _formgenUtils = SupportTool.SupportToolInstance.FormgenUtils;
}
