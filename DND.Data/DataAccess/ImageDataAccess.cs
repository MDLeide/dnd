using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Cashew.Data;
using DND.DataAccess;
using DND.Domain;

namespace DND.Data.DataAccess
{
    public class ImageDataAccess : DataAccess<Image>, IImageDataAccess
    {
        protected override string InsertStatement => Statements.Images.Insert;
        protected override string UpdateStatement => Statements.Images.Update;
        protected override string DeleteStatement => Statements.Images.Delete;
        protected override string GetByIdStatement => Statements.Images.SelectById;
        protected override string GetAllStatement => Statements.Images.SelectAll;


        public IEnumerable<Image> GetByCardId(int id)
        {
            var cmd = GetCommand();
            cmd.CommandText = Statements.Images.SelectByCardId;
            cmd.Parameters.AddWithValue("@id", id);
            return ExecuteSelectCommand(cmd, Parse, true);
        }

        public void SaveImageToCard(Image image, Card card)
        {
            if (!card.Id.HasValue)
                throw new InvalidOperationException("Cannot save an Image to a Card that does not have an Id.");

            if (!image.Id.HasValue)
                Save(image);
            
            if (Exists(image, card))
                return;

            var cmd = GetCommand();
            cmd.CommandText = Statements.CardImages.Insert;
            cmd.Parameters.AddWithValue("@cardId", card.Id.Value);
            // ReSharper disable once PossibleInvalidOperationException
            cmd.Parameters.AddWithValue("@imageId", image.Id.Value);
            cmd.ExecuteNonQuery();
        }

        public override void Delete(Image obj)
        {
            // todo: remove image from any objects using it
            base.Delete(obj);
        }

        bool Exists(Image image, Card card)
        {
            if (!image.Id.HasValue)
                throw new InvalidOperationException("Card to Image relationship cannot exist for Image without Id.");
            if (!card.Id.HasValue)
                throw new InvalidOperationException("Card to Image relationship cannot exist for Card without Id.");

            var cmd = GetCommand();
            cmd.CommandText = Statements.CardImages.Exists;
            cmd.Parameters.AddWithValue("@imageId", image.Id.Value);
            cmd.Parameters.AddWithValue("@cardId", card.Id.Value);
            return (bool)cmd.ExecuteScalar();
        }

        protected override void ValidateSave(Image image)
        {
            
        }

        protected override void AddParametersToInsertUpdateCommand(SQLiteCommand cmd, Image image)
        {
            if (image.Id.HasValue)
                cmd.Parameters.AddWithValue("@id", image.Id.Value);

            cmd.Parameters.AddWithValue("@location", image.Location);
            cmd.Parameters.AddWithValue("@name", image.Name);
            cmd.Parameters.AddWithValue("@description", image.Description);
            cmd.Parameters.AddWithValue("@size", image.Size);
        }
        
        protected override Image Parse(IDataRecord record)
        {
            var image = new Image();
            image.Id = record.ToInt("Id");
            image.Description = record.ToString("Description");
            image.Name = record.ToString("Name");
            image.Location = record.ToString("Location");
            image.Size = record.ToInt("Size");
            return image;
        }
    }
}