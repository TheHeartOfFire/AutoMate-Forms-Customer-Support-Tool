using AMFormsCST.Core.Interfaces.UserSettings;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Appearance;

namespace AMFormsCST.Desktop.Models.UserSettings
{
    public partial class NewTemplateSetting : ObservableObject, ISetting
    {
        [ObservableProperty]
        private bool _selectNewTemplate = true;
    }
    
}
