
namespace AMFormsCST.Desktop.Models;

public interface ISelectable
{
    Guid Id { get; }
    bool IsSelected { get; }
    public void Select();
    public void Deselect();
}