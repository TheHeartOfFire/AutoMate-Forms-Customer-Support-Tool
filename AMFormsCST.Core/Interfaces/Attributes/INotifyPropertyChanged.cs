namespace AMFormsCST.Core.Interfaces.Attributes;
public interface INotifyPropertyChanged
{
    event EventHandler? PropertyChanged;
    void OnPropertyChanged();
}
