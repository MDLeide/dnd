using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using DND.Domain;

namespace DND.UI.Domain
{
    public class CardTypeViewModel : DomainViewModel<DND.Domain.CardType>
    {
        public CardTypeViewModel(
            DND.Domain.CardType cardType,
            IEnumerable<DND.Domain.PropertyType> propertyTypes = null)
            : base(cardType, DataAccessManager.CardTypeDataAccess)
        {
            IEnumerable<PropertyTypeViewModel> propTypeVms;

            if (propertyTypes != null)
                propTypeVms = propertyTypes.Select(p => new PropertyTypeViewModel(p)).ToArray();
            else
                propTypeVms = new PropertyTypeViewModel[] { };

            PropertyTypes =
                new CheckableCollection<DND.Domain.PropertyType, PropertyTypeViewModel>(
                    cardType.PropertyTypes,
                    propTypeVms);
        }

        public DND.Domain.CardType CardType => Object;
        
        public string Name
        {
            get => Object.Name;
            set
            {
                Object.Name = value;
                NotifyOfPropertyChange(nameof(Name));
            }
        }

        public string Description
        {
            get => Object.Description;
            set
            {
                Object.Description = value;
                NotifyOfPropertyChange(nameof(Description));
            }
        }

        ImageViewModel _image;
        public ImageViewModel Image
        {
            get => _image ?? (CardType.Image == null ? null : _image = new ImageViewModel(CardType.Image));
            set
            {
                if (Equals(value, _image)) return;
                _image = value;
                NotifyOfPropertyChange(nameof(Image));
            }
        }

        CheckableCollection<DND.Domain.PropertyType, PropertyTypeViewModel> _propertyTypes;
        public CheckableCollection<DND.Domain.PropertyType, PropertyTypeViewModel> PropertyTypes
        {
            get => _propertyTypes;
            set
            {
                if (Equals(value, _propertyTypes)) return;
                _propertyTypes = value;
                NotifyOfPropertyChange(nameof(PropertyTypes));
            }
        }

        protected override void PostSave()
        {
            foreach (var removed in PropertyTypes.Removed)
                DataAccessManager.CardTypeDataAccess.RemovePropertyTypeFromCardType(CardType, removed.PropertyType);
            PropertyTypes.AcceptChanges();
        }
    }
}
