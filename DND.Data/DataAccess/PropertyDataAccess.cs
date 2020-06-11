using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Cashew.Data;
using DND.Data.DataAccess.Wrappers;
using DND.DataAccess;
using DND.Domain;

namespace DND.Data.DataAccess
{
    public class PropertyDataAccess : DataAccess<Property>, IPropertyDataAccess
    {
        public PropertyDataAccess(ICardDataAccess cardDataAccess, IPropertyTypeDataAccess propertyTypeDataAccess)
        {
            CardDataAccess = cardDataAccess;
            PropertyTypeDataAccess = propertyTypeDataAccess;
        }


        public ICardDataAccess CardDataAccess { get; set; }
        public IPropertyTypeDataAccess PropertyTypeDataAccess { get; set; }

        protected override string InsertStatement => Statements.Properties.Insert;
        protected override string UpdateStatement => Statements.Properties.Update;
        protected override string DeleteStatement => Statements.Properties.Delete;
        protected override string GetByIdStatement => null;
        protected override string GetAllStatement => null;


        public IEnumerable<Property> GetPropertiesByCardId(int id)
        {
            var cmd = GetCommand();
            cmd.CommandText = Statements.Properties.SelectByCardId;
            cmd.Parameters.AddWithValue("@id", id);
            return ExecuteSelectCommand(cmd, Parse, true);
        }


        protected override void ValidateSave(Property property)
        {
            if (property.Owner == null)
                throw new InvalidOperationException("Property must have an owner to be saved.");

            if (!property.Owner.Id.HasValue)
                throw new InvalidOperationException("Card must be saved before its properties.");

            if (property.PropertyType == null)
                throw new InvalidOperationException("Property must have a property type to be saved.");

            if (!property.PropertyType.Id.HasValue)
                throw new InvalidOperationException("Property type must be saved before a property using it can be saved.");
        }
        
        protected override void AddParametersToInsertUpdateCommand(SQLiteCommand cmd, Property property)
        {
            if (property.PropertyType?.Id == null)
                throw new InvalidOperationException("Property must have a Property Type with an Id.");

            if (property.Owner?.Id == null)
                throw new InvalidOperationException("Property must have an owner (Domain.Card) with an Id.");

            cmd.Parameters.AddWithValue("@propertyTypeId", property.PropertyType.Id.Value);
            cmd.Parameters.AddWithValue("@cardId", property.Owner.Id.Value);
            cmd.Parameters.AddWithValue("@val", property.Value);
        }

        protected override Property Parse(IDataRecord record)
        {
            var ownerId = record.ToInt("CardId");
            var propertyTypeId = record.ToInt("PropertyTypeId");
            var property = new PropertyWrapper(
                CardDataAccess,
                PropertyTypeDataAccess,
                ownerId,
                propertyTypeId);
            property.Id = record.ToInt("Id");
            property.Value = record.ToString("Value");
            return property;
        }
    }
}