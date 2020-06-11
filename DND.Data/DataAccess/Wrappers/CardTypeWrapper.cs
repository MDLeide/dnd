using System.Collections.Generic;
using Cashew.Data;
using DND.DataAccess;
using DND.Domain;

namespace DND.Data.DataAccess.Wrappers
{
    class CardTypeWrapper : CardType
    {
        public CardTypeWrapper(IImageDataAccess imageDataAccess, IPropertyTypeDataAccess propertyTypeDataAccess, int cardTypeId, int imageId)
        {
            Id = cardTypeId;
            _image = new PropWrapper<Image>(() => imageDataAccess.Get(imageId));
            _propertyTypes = new PropWrapper<List<PropertyType>>(() =>
                new List<PropertyType>(propertyTypeDataAccess.GetPropertyTypesByCardTypeId(Id.Value)));
        }

        readonly PropWrapper<Image> _image;
        public override Image Image
        {
            get => _image.Value;
            set => _image.Value = value;
        }

        readonly PropWrapper<List<PropertyType>> _propertyTypes;
        public override List<PropertyType> PropertyTypes
        {
            get => _propertyTypes.Value;
            set => _propertyTypes.Value = value;
        }
    }
}