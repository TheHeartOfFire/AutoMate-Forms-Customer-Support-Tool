using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Interfaces.Attributes;
public interface INotifyPropertyChanged
{
    event EventHandler? PropertyChanged;
    void OnPropertyChanged();
}
