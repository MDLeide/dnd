using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cashew.UI.WPF.MVVM;
using Cashew.UI.WPF.MVVM.PropertyChange;
using DND.Domain;
using DND.UI.Domain;

namespace DND.UI.Screens.Select
{
    public abstract class SelectorViewModelBase<TDomain, TViewModel> : Screen
        where TDomain : DomainObject
        where TViewModel : DomainViewModel<TDomain>
    {
        protected SelectorViewModelBase(TDomain selectedItem)
            : base(true, selectedItem) { }

        protected SelectorViewModelBase(TViewModel selectedItem)
            : base(true, selectedItem) { }

        ObservableCollection<TViewModel> _items;
        public ObservableCollection<TViewModel> Items
        {
            get => _items;
            set
            {
                if (Equals(value, _items)) return;
                _items = value;
                NotifyOfPropertyChange(nameof(Items));
            }
        }

        TViewModel _selectedItem;
        public TViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (Equals(value, _selectedItem)) return;
                _selectedItem = value;
                NotifyOfPropertyChange(nameof(SelectedItem));
            }
        }

        [NotifiedBy(nameof(SelectedItem))]
        public bool CanOkay => SelectedItem != null;

        public void Okay()
        {
            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }

        protected abstract IEnumerable<TViewModel> GetItems();

        protected override object OnLoad(object context)
        {
            return GetItems();
        }

        protected override void PostLoad(object context, object loadContext)
        {
            Items = new ObservableCollection<TViewModel>((IEnumerable<TViewModel>) loadContext);
            if (context is TDomain obj)
                SelectedItem = Items.FirstOrDefault(p => p.Id == obj.Id);
            else if (context is TViewModel vm)
                SelectedItem = Items.FirstOrDefault(p => p.Id == vm.Id);
        }
    }
}
