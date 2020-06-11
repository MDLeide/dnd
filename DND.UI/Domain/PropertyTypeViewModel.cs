using DND.Domain;

namespace DND.UI.Domain
{
    public class PropertyTypeViewModel : DomainViewModel<DND.Domain.PropertyType>
    {
        public PropertyTypeViewModel(DND.Domain.PropertyType propertyType)
            : base(propertyType, DataAccessManager.PropertyTypeDataAccess) { }

        public DND.Domain.PropertyType PropertyType => Object;

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
    }
}