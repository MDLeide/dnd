using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cashew.Data;
using DND.Data.DataAccess.Wrappers;
using DND.DataAccess;
using DND.Domain;

namespace DND.Data.DataAccess
{
    public class CardSpaceDataAccess : DataAccess<CardSpace>, ICardSpaceDataAccess
    {
        public CardSpaceDataAccess(IImageDataAccess imageDataAccess)
        {
            ImageDataAccess = imageDataAccess;
        }

        public IImageDataAccess ImageDataAccess { get; }

        protected override string InsertStatement => Statements.CardSpaces.Insert;
        protected override string UpdateStatement => Statements.CardSpaces.Update;
        protected override string DeleteStatement => Statements.CardSpaces.Delete;
        protected override string GetByIdStatement => Statements.CardSpaces.SelectById;
        protected override string GetAllStatement => Statements.CardSpaces.SelectAll;

        protected override void ValidateSave(CardSpace obj)
        {
            if (string.IsNullOrEmpty(obj.Name))
                throw new InvalidOperationException("Card space must have a name and it must be unique.");
        }
        
        // todo: save card space cards
        // todo: load card space cards
        protected override CardSpace Parse(IDataRecord record)
        {
            var space = new CardSpaceWrapper(ImageDataAccess, record.ToNullableInt("BackgroundImageId"));
            space.Id = record.ToInt("Id");
            space.Name = record.ToString("Name");
            space.Description = record.ToString("Description");
            space.BackgroundColorCode = record.ToString("BackgroundColorCode");
            return space;
        }

        protected override void AddParametersToInsertUpdateCommand(SQLiteCommand cmd, CardSpace space)
        {
            cmd.Parameters.AddWithValue("@name", space.Name);
            cmd.Parameters.AddWithValue("@description", space.Description);
            cmd.Parameters.AddWithValue("@backgroundColorCode", space.BackgroundColorCode);
            cmd.Parameters.AddWithValue("@backgroundImageId", space.BackgroundImage?.Id);
        }
    }
}
