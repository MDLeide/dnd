using System.Collections.Generic;
using Cashew.Data;
using DND.DataAccess;
using DND.Domain;

namespace DND.Data.DataAccess.Wrappers
{
    class CardWrapper : Card
    {
        public CardWrapper(
            ICardTypeDataAccess cardTypeDataAccess,
            IImageDataAccess imageDataAccess,
            IHardLinkDataAccess hardLinkDataAccess,
            IPropertyDataAccess propertyDataAccess,
            int cardId,
            int cardTypeId,
            int? primaryImageId)
        {
            _cardType = new PropWrapper<CardType>( 
                () => cardTypeDataAccess.Get(cardTypeId));

            _primaryImage = new PropWrapper<Image>(
                () => primaryImageId.HasValue ? imageDataAccess.Get(primaryImageId.Value) : null);

            _additionalImages = new PropWrapper<List<Image>>(
                () => new List<Image>(imageDataAccess.GetByCardId(cardId)));

            _hardLinks = new PropWrapper<List<HardLink>>(
                () => new List<HardLink>(hardLinkDataAccess.GetHardLinksByOriginId(cardId)));

            _referencedBy = new PropWrapper<List<HardLink>>(
                () => new List<HardLink>(hardLinkDataAccess.GetHardLinksByTargetId(cardId)));

            _props = new PropWrapper<List<Property>>(
                () => new List<Property>(propertyDataAccess.GetPropertiesByCardId(cardId)));
        }

        readonly PropWrapper<CardType> _cardType;
        public override CardType CardType
        {
            get => _cardType.Value;
            set => _cardType.Value = value;
        }

        readonly PropWrapper<Image> _primaryImage;
        public override Image PrimaryImage
        {
            get => _primaryImage.Value;
            set => _primaryImage.Value = value;
        }

        readonly PropWrapper<List<Image>> _additionalImages;
        public override List<Image> AdditionalImages
        {
            get => _additionalImages.Value;
            set => _additionalImages.Value = value;
        }

        readonly PropWrapper<List<HardLink>> _hardLinks;
        public override List<HardLink> HardLinks
        {
            get => _hardLinks.Value;
            set => _hardLinks.Value = value;
        }

        readonly PropWrapper<List<HardLink>> _referencedBy;
        public override List<HardLink> ReferencedBy
        {
            get => _referencedBy.Value;
            set => _referencedBy.Value = value;
        }

        readonly PropWrapper<List<Property>> _props;
        public override List<Property> Properties
        {
            get => _props.Value;
            set => _props.Value = value;
        }
    }
}