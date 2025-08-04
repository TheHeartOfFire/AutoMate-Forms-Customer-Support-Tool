using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Interfaces.UserSettings;
public interface IUserSettings : ISetting
{
    IOrgVariables Organization { get; set; }
    string Name { get; set; }
    string ApplicationFilesPath { get; set; }
    string ExtSeparator { get; set; }
}
