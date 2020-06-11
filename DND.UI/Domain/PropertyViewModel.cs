using DND.Domain;

namespace DND.UI.Domain
{
    class PropertyViewModel : DomainViewModel<Property>
    {
        public PropertyViewModel(Property property)
            : base(property, DataAccessManager.PropertyDataAccess) { }

        public Property Property => Object;

        public CardViewModel CardViewModel
        {
            get => _cardViewModel ?? (_cardViewModel = new CardViewModel(Property.Owner));
            set
            {
                if (Equals(value, _cardViewModel)) return;
                _cardViewModel = value;
                NotifyOfPropertyChange(nameof(CardViewModel));
            }
        }

        CardViewModel _cardViewModel;

        public PropertyTypeViewModel PropertyType
        {
            get => _propertyType ?? (_propertyType = new PropertyTypeViewModel(Property.PropertyType));
            set
            {
                if (Equals(value, _propertyType)) return;
                _propertyType = value;
                NotifyOfPropertyChange(nameof(PropertyType));
            }
        }

        PropertyTypeViewModel _propertyType;

        public string Value
        {
            get => Object.Value;
            set
            {
                Object.Value = value;
                NotifyOfPropertyChange(nameof(Value));
            }
        }
    }
}