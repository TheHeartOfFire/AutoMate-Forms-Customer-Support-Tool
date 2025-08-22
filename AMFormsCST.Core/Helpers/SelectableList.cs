using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Helpers;
public class SelectableList<T> : List<T>
    where T : class
{
    public T? SelectedItem { get; set; }
    public SelectableList() : base() { }
    public SelectableList(IEnumerable<T> collection) : base(collection) { }
    public SelectableList(int capacity) : base(capacity) { }
    public void Select(T item)
    {
        if (Contains(item))
        {
            SelectedItem = item;
        }
        else
        {
            throw new ArgumentException("Item not found in the list.", nameof(item));
        }
    }
}
