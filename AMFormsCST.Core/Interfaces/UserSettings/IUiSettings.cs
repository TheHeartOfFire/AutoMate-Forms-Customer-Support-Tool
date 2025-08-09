using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Interfaces.UserSettings;
public interface IUiSettings : ISetting
{
    List<ISetting> Settings { get; set; }
}
