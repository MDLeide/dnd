using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cashew.UI.WPF.MVVM;
using DND.Domain;

namespace DND.UI.Domain
{
    public class CheckableCollection<TDomain, TViewModel> : Screen
        where TDomain : DomainObject
        where TViewModel : DomainViewModel<TDomain>
    {
        readonly IEnumerable<TDomain> _original;
        readonly IList<TViewModel> _removed = new List<TViewModel>();
        readonly IList<TViewModel> _added = new List<TViewModel>();

        
        public CheckableCollection(IList<TDomain> source, IEnumerable<TViewModel> all)
        {
            _original = source.ToArray();
            Items = Wrap(source, all);
            Register(source, Items);
        }
        
        ObservableCollection<CheckableWrapper<TDomain, TViewModel>> _items =
            new ObservableCollection<CheckableWrapper<TDomain, TViewModel>>();

        public ObservableCollection<CheckableWrapper<TDomain, TViewModel>> Items
        {
            get => _items;
            set
            {
                if (Equals(value, _items)) return;
                _items = value;
                NotifyOfPropertyChange(nameof(Items));
            }
        }

        public IEnumerable<TViewModel> Removed => _removed;
        public IEnumerable<TViewModel> Added => _added;

        public void AcceptChanges()
        {
            _removed.Clear();
            _added.Clear();
            foreach (var item in Items)
                item.AcceptChanges();
        }

        void Register(IList<TDomain> source, IEnumerable<CheckableWrapper<TDomain, TViewModel>> collection)
        {
            foreach (var item in collection)
                item.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName != nameof(CheckableWrapper<TDomain, TViewModel>.IsChecked))
                        return;

                    if (!(s is CheckableWrapper<TDomain, TViewModel> vm))
                        return;

                    if (vm.IsChecked)
                    {
                        source.Add(vm.Value.Object);
                        if (_removed.Contains(vm.Value))
                            _removed.Remove(vm.Value);
                        if (!_original.Contains(vm.Value.Object))
                            _added.Add(vm.Value);
                    }
                    else
                    {
                        source.Remove(vm.Value.Object);
                        if (_original.Contains(vm.Value.Object))
                            _removed.Add(vm.Value);
                        if (_added.Contains(vm.Value))
                            _added.Remove(vm.Value);
                    }
                };
        }

        static ObservableCollection<CheckableWrapper<TDomain, TViewModel>> Wrap(IList<TDomain> source, IEnumerable<TViewModel> all)
        {
            var items = new ObservableCollection<CheckableWrapper<TDomain, TViewModel>>();

            foreach (var vm in all)
            {
                var exists = source.Any(p => p.Id == vm.Object.Id);
                items.Add(new CheckableWrapper<TDomain, TViewModel>(vm, exists));
            }

            return items;
        }
    }
}