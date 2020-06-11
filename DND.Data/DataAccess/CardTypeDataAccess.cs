using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Cashew.Data;
using DND.Data.DataAccess.Wrappers;
using DND.DataAccess;
using DND.Domain;

namespace DND.Data.DataAccess
{
    public class CardTypeDataAccess : DataAccess<CardType>, ICardTypeDataAccess
    {
        public CardTypeDataAccess(IImageDataAccess imageDataAccess, IPropertyTypeDataAccess propertyTypeDataAccess)
        {
            ImageDataAccess = imageDataAccess;
            PropertyTypeDataAccess = propertyTypeDataAccess;
        }


        public IImageDataAccess ImageDataAccess { get; set; }
        public IPropertyTypeDataAccess PropertyTypeDataAccess { get; set; }

        protected override string InsertStatement => Statements.CardTypes.Insert;
        protected override string UpdateStatement => Statements.CardTypes.Update;
        protected override string DeleteStatement => Statements.CardTypes.Delete;
        protected override string GetByIdStatement => Statements.CardTypes.SelectById;
        protected override string GetAllStatement => Statements.CardTypes.SelectAll;


        public IEnumerable<CardType> GetCardTypesByPropertyTypeId(int propertyTypeId)
        {
            var cmd = GetCommand();
            cmd.CommandText = Statements.CardTypes.SelectByPropertyTypeId;
            cmd.Parameters.AddWithValue("@id", propertyTypeId);
            return ExecuteSelectCommand(cmd, Parse, true);
        }

        public void RemovePropertyTypeFromCardType(CardType cardType, PropertyType propertyType)
        {
            var cmd = GetCommand();
            cmd.CommandText = Statements.PropertyTypeCardTypes.DeleteByIds;
            cmd.Parameters.AddWithValue("@propertyTypeId", propertyType.Id);
            cmd.Parameters.AddWithValue("@cardTypeId", cardType.Id);
            cmd.ExecuteNonQuery();
        }
        
        protected override void ValidateSave(CardType type)
        {
            // no op
        }

        protected override void PostSave(CardType obj)
        {
            if (!obj.Id.HasValue)
                throw new InvalidOperationException();

            foreach (var propType in obj.PropertyTypes)
                PropertyTypeDataAccess.Save(propType);

            foreach (var propType in obj.PropertyTypes.Where(p => !Exists(obj.Id, p.Id)))
                AddPropertyTypeToCardType(obj.Id.Value, propType.Id.Value);
        }

        protected override void AddParametersToInsertUpdateCommand(SQLiteCommand cmd, CardType type)
        {
            cmd.Parameters.AddWithValue("@name", type.Name);
            cmd.Parameters.AddWithValue("@description", type.Description);
            cmd.Parameters.AddWithValue("@imageId", type.Image?.Id);
        }

        protected override CardType Parse(IDataRecord record)
        {
            var imageId = record.ToInt("ImageId");
            var id = record.ToInt("Id");
            var type = new CardTypeWrapper(ImageDataAccess, PropertyTypeDataAccess, id, imageId);
            type.Id = id;
            type.Name = record.ToString("Name");
            type.Description = record.ToString("Description");
            return type;
        }

        void AddPropertyTypeToCardType(int cardTypeId, int propertyTypeId)
        {
            var cmd = GetCommand();
            cmd.CommandText = Statements.PropertyTypeCardTypes.Insert;
            cmd.Parameters.AddWithValue("@cardTypeId", cardTypeId);
            cmd.Parameters.AddWithValue("@propertyTypeId", propertyTypeId);
            cmd.ExecuteNonQuery();
        }

        bool Exists(int? cardTypeId, int? propertyTypeId)
        {
            if (cardTypeId == null || propertyTypeId == null)
                return false;

            var cmd = GetCommand();
            cmd.CommandText = Statements.PropertyTypeCardTypes.Exists;
            cmd.Parameters.AddWithValue("@cardTypeId", cardTypeId);
            cmd.Parameters.AddWithValue("@propertyTypeId", propertyTypeId);
            var response = cmd.ExecuteScalar();
            return (long) response == 1;
        }
    }
}