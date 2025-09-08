using System.Windows;

namespace AMFormsCST.Desktop.Interfaces;
public interface IWindow
{
    event RoutedEventHandler Loaded;

    void Show();
}
