using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cashew.UI.WPF.MVVM;
using Cashew.UI.WPF.MVVM.PropertyChange;
using DND.DataAccess;
using DND.Domain;
using DND.UI.Components.IO;
using DND.UI.Domain;

namespace DND.UI.Screens.Configure
{
    public class PropertyTypeManagementViewModel : Screen
    {
        public IPropertyTypeDataAccess DataAccess { get; } = DataAccessManager.PropertyTypeDataAccess;

        PropertyTypeViewModel _selectedPropertyType;
        public PropertyTypeViewModel SelectedPropertyType
        {
            get => _selectedPropertyType;
            set
            {
                if (Equals(value, _selectedPropertyType)) return;
                _selectedPropertyType = value;
                NotifyOfPropertyChange(nameof(SelectedPropertyType));
            }
        }

        ObservableCollection<PropertyTypeViewModel> _propertyTypes;
        public ObservableCollection<PropertyTypeViewModel> PropertyTypes
        {
            get => _propertyTypes;
            set
            {
                if (Equals(value, _propertyTypes)) return;
                _propertyTypes = value;
                NotifyOfPropertyChange(nameof(PropertyTypes));
            }
        }

        public void NewPropertyType()
        {
            //todo: what if unsaved property type?

            var type = new PropertyType();
            type.Name = "New Property Type";
            type.Description = "Description";
            var typeVm = new PropertyTypeViewModel(type);

            PropertyTypes.Add(typeVm);
            SelectedPropertyType = typeVm;
        }

        [NotifiedBy(nameof(SelectedPropertyType))]
        public bool CanSave => SelectedPropertyType != null;

        public void Save()
        {
            SelectedPropertyType.Save();
        }

        [NotifiedBy(nameof(SelectedPropertyType))]
        public bool CanDelete => SelectedPropertyType != null;

        public void Delete()
        {
            SelectedPropertyType.Delete();
            PropertyTypes .Remove(SelectedPropertyType);
            SelectedPropertyType = null;
        }

        protected override object OnLoad(object context) => 
            DataAccess.Get().Select(p => new PropertyTypeViewModel(p));

        protected override void PostLoad(object context, object loadContext) =>
            PropertyTypes = new ObservableCollection<PropertyTypeViewModel>((IEnumerable<PropertyTypeViewModel>) loadContext);
    }
}
