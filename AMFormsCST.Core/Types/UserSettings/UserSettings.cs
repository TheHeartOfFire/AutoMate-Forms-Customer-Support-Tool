using AMFormsCST.Core.Interfaces.UserSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Types.UserSettings;
public class UserSettings(IOrgVariables org) : IUserSettings
{
    public IOrgVariables Organization { get; set; } = org;
    public string Name { get; set; } = string.Empty;
    public string ApplicationFilesPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    public string ExtSeparator { get; set; } = "x ";
}
