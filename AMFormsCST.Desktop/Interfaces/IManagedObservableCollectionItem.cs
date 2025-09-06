using AMFormsCST.Desktop.Models;
using AMFormsCST.Desktop.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AMFormsCST.Desktop.Interfaces
{
    public interface IManagedObservableCollectionItem : IBlankMaybe, INotifyPropertyChanged
    {
        public Guid Id { get; }
        public CollectionMemberState State { get; }
        public event EventHandler<GuidEventArgs>? Selected;
        public void Select();
        public void Select(Guid? guid);
        public void OnAddedToCollection(IManagedObservableCollection containingCollection);
        public void OnRemovedFromCollection();

        public enum CollectionMemberState
        {
            NotACollectionMember,
            NotSelected,
            Selected
        }
    }
}
