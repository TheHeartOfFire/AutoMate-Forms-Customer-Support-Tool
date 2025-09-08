using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.Extensions;
public static class TreeViewItemExtensions
{
    public static void ExpandAll(this System.Windows.Controls.TreeViewItem item)
    {
        item.IsExpanded = true;
        foreach (System.Windows.Controls.TreeViewItem child in item.Items)
        {
            child.ExpandAll();
        }
    }
    public static void CollapseAll(this System.Windows.Controls.TreeViewItem item)
    {
        item.IsExpanded = false;
        foreach (System.Windows.Controls.TreeViewItem child in item.Items)
        {
            child.CollapseAll();
        }
    }
    public static TreeViewItem Clone(this TreeViewItem item)
    {
        var newItem = new TreeViewItem
        {
            Header = item.Header,
            Tag = item.Tag,
            IsExpanded = item.IsExpanded
        };
        foreach (TreeViewItem child in item.Items)
        {
            newItem.Items.Add(child.Clone());
        }
        return newItem;
    }
}
