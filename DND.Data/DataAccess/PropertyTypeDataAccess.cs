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
    public class PropertyTypeDataAccess : DataAccess<PropertyType>, IPropertyTypeDataAccess
    {
        protected override string InsertStatement => Statements.PropertyTypes.Insert;
        protected override string UpdateStatement => Statements.PropertyTypes.Update;
        protected override string DeleteStatement => Statements.PropertyTypes.Delete;
        protected override string GetByIdStatement => Statements.PropertyTypes.SelectById;
        protected override string GetAllStatement => Statements.PropertyTypes.SelectAll;


        public IEnumerable<PropertyType> GetPropertyTypesByCardTypeId(int id)
        {
            var cmd = GetCommand();
            cmd.CommandText = Statements.PropertyTypes.SelectByCardTypeId;
            cmd.Parameters.AddWithValue("@id", id);
            return ExecuteSelectCommand(cmd, Parse, true);
        }

        protected override void ValidateSave(PropertyType type) { }

        protected override void AddParametersToInsertUpdateCommand(SQLiteCommand cmd, PropertyType type)
        {
            cmd.Parameters.AddWithValue("@name", type.Name);
            cmd.Parameters.AddWithValue("@description", type.Description);
        }
        
        protected override PropertyType Parse(IDataRecord record)
        {
            var type = new PropertyType();
            type.Id = record.ToInt("Id");
            type.Description = record.ToString("Description");
            type.Name = record.ToString("Name");
            return type;
        }
    }
}