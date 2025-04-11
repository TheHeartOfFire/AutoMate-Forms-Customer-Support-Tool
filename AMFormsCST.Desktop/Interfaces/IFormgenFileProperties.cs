using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AMFormsCST.Desktop.Interfaces;
public interface IFormgenFileProperties
{
    public IFormgenFileSettings Settings { get; set; }

    public UIElement GetUIElements();
}
