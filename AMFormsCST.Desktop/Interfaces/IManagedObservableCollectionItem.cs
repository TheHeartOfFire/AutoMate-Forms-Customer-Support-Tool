using AMFormsCST.Desktop.Types;
using System.ComponentModel;

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
