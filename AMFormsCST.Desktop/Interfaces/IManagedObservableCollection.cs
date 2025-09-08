using AMFormsCST.Desktop.Types;

namespace AMFormsCST.Desktop.Interfaces;
public interface IManagedObservableCollection
{
    event EventHandler<GuidEventArgs>? SelectionChanged;
}