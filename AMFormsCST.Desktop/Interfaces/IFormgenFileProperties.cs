using AMFormsCST.Desktop.Models.FormgenUtilities;
using System.Collections.Generic;
using System.Windows;

namespace AMFormsCST.Desktop.Interfaces;

public interface IFormgenFileProperties
{
    IEnumerable<DisplayProperty> GetDisplayProperties();
}
