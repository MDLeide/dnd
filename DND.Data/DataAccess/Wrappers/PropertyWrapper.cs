using System;
using Cashew.Data;
using DND.DataAccess;
using DND.Domain;

namespace DND.Data.DataAccess.Wrappers
{
    class PropertyWrapper : Property
    {

        public PropertyWrapper(
            ICardDataAccess cardDataAccess, 
            IPropertyTypeDataAccess propertyTypeDataAccess,
            int ownerId, 
            int propertyTypeId)
        {
            _owner = new PropWrapper<Card>(() => cardDataAccess.Get(ownerId));
            _propertyType = new PropWrapper<PropertyType>(() => propertyTypeDataAccess.Get(propertyTypeId));
        }

        readonly PropWrapper<Card> _owner;

        public override Card Owner
        {
            get => _owner.Value;
            set => _owner.Value = value;
        }

        readonly PropWrapper<PropertyType> _propertyType;

        public override PropertyType PropertyType
        {
            get => _propertyType.Value;
            set => _propertyType.Value = value;
        }
    }
}