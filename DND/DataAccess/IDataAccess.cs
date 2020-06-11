using System.Collections.Generic;
using DND.Domain;

namespace DND.DataAccess
{
    public interface IDataAccess<T>
    {
        IEnumerable<T> Get();
        T Get(int id);
        void Save(T obj);
        void Delete(T obj);
    }

    public interface ICardSpaceDataAccess : IDataAccess<CardSpace> { }

    public interface ICardSpaceCardDataAccess : IDataAccess<CardSpaceCard>
    {
        CardSpaceCard GetByCardAndCardSpaceId(int cardId, int cardSpaceId);
        IEnumerable<CardSpaceCard> GetByCardSpaceId(int cardSpaceId);
    }

    public interface ICardDataAccess : IDataAccess<Card>
    {
        Card GetCardByTitle(string title);
        IEnumerable<Card> GetCardsByTitleStartsWith(string title, bool caseSensitive);
        IEnumerable<Card> GetCardsByTitleContains(string title, bool caseSensitive);
    }

    public interface ICardTypeDataAccess : IDataAccess<CardType>
    {
        IEnumerable<CardType> GetCardTypesByPropertyTypeId(int id);
        void RemovePropertyTypeFromCardType(CardType cardType, PropertyType propertyType);
    }

    public interface IHardLinkDataAccess : IDataAccess<HardLink>
    {
        IEnumerable<HardLink> GetHardLinksByOriginId(int id);
        IEnumerable<HardLink> GetHardLinksByTargetId(int id);
    }

    public interface IImageDataAccess : IDataAccess<Image>
    {
        IEnumerable<Image> GetByCardId(int id);
        void SaveImageToCard(Image image, Card card);
    }

    public interface IPropertyDataAccess : IDataAccess<Property>
    {
        IEnumerable<Property> GetPropertiesByCardId(int id);
    }

    public interface IPropertyTypeDataAccess : IDataAccess<PropertyType>
    {
        IEnumerable<PropertyType> GetPropertyTypesByCardTypeId(int id);
    }
}