using System;
using System.Collections.Generic;
using DND.Domain;
using DND.UI.Domain;

namespace DND.UI.Screens.Select
{
    public class SelectorViewModel<TDomain, TViewModel> : SelectorViewModelBase<TDomain, TViewModel>
        where TDomain : DomainObject
        where TViewModel : DomainViewModel<TDomain>
    {
        readonly Func<IEnumerable<TViewModel>> _getViewModels;
        IEnumerable<TViewModel> _viewModels;

        public SelectorViewModel(IEnumerable<TViewModel> viewModels, TViewModel selectedItem)
            : base(selectedItem)
        {
            _viewModels = viewModels;
        }

        public SelectorViewModel(IEnumerable<TViewModel> viewModels, TDomain selectedItem)
            : base(selectedItem)
        {
            _viewModels = viewModels;
        }

        public SelectorViewModel(Func<IEnumerable<TViewModel>> getViewModels, TViewModel selectedItem)
            : base(selectedItem)
        {
            _getViewModels = getViewModels;
        }

        public SelectorViewModel(Func<IEnumerable<TViewModel>> getViewModels, TDomain selectedItem)
            : base(selectedItem)
        {
            _getViewModels = getViewModels;
        }

        protected override IEnumerable<TViewModel> GetItems()
        {
            if (_viewModels == null)
                _viewModels = _getViewModels();
            return _viewModels;
        }
    }
}