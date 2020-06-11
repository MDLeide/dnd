using Cashew.UI.WPF.MVVM;
using DND.Domain;

namespace DND.UI.Domain
{
    public class CheckableWrapper<TDomain, TViewModel> : Screen
        where TDomain : DomainObject
        where TViewModel : DomainViewModel<TDomain>
    {
        bool _originalIsChecked;

        public CheckableWrapper(TViewModel val, bool isChecked)
        {
            Value = val;
            IsChecked = isChecked;
            _originalIsChecked = isChecked;
            if (val.Object is INamed asNamed)
                Name = asNamed.Name;
            else
                Name = val.DisplayName;
        }

        public bool IsChanged => _originalIsChecked != IsChecked;
        string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (Equals(value, _name)) return;
                _name = value;
                NotifyOfPropertyChange(nameof(Name));
            }
        }

        bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (Equals(value, _isChecked)) return;
                _isChecked = value;
                NotifyOfPropertyChange(nameof(IsChecked));
            }
        }

        TViewModel _value;
        public TViewModel Value
        {
            get => _value;
            set
            {
                if (Equals(value, _value)) return;
                _value = value;
                NotifyOfPropertyChange(nameof(Value));
            }
        }

        public void DiscardChanges()
        {
            IsChecked = _originalIsChecked;
        }

        public void AcceptChanges()
        {
            _originalIsChecked = IsChecked;
        }
    }
}
