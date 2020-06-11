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
    public class CardDataAccess : DataAccess<Card>, ICardDataAccess
    {
        public CardDataAccess(
            ICardTypeDataAccess cardTypeDataAccess, 
            IHardLinkDataAccess hardLinkDataAccess, 
            IImageDataAccess imageDataAccess,
            IPropertyDataAccess propertyDataAccess)
        {
            CardTypeDataAccess = cardTypeDataAccess;
            HardLinkDataAccess = hardLinkDataAccess;
            ImageDataAccess = imageDataAccess;
            PropertyDataAccess = propertyDataAccess;
        }


        public ICardTypeDataAccess CardTypeDataAccess { get; set; }
        public IHardLinkDataAccess HardLinkDataAccess { get; set; }
        public IImageDataAccess ImageDataAccess { get; set; }
        public IPropertyDataAccess PropertyDataAccess { get; set; }

        protected override string InsertStatement => Statements.Cards.Insert;
        protected override string UpdateStatement => Statements.Cards.Update;
        protected override string DeleteStatement => Statements.Cards.Delete;
        protected override string GetByIdStatement => Statements.Cards.SelectById;
        protected override string GetAllStatement => Statements.Cards.SelectAll;


        public Card GetCardByTitle(string title)
        {
            var cmd = GetCommand();
            cmd.Parameters.AddWithValue("@title", title);
            cmd.CommandText = Statements.Cards.SelectByTitle;
            return ExecuteSelectCommand(cmd, Parse, true).FirstOrDefault();
        }

        public IEnumerable<Card> GetCardsByTitleStartsWith(string title, bool caseSensitive)
        {
            var cmd = GetCommand();
            cmd.CommandText = Statements.Cards.SelectByTitleStartsWith;
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@caseSensitive", caseSensitive);
            return ExecuteSelectCommand(cmd, Parse, true);
        }

        public IEnumerable<Card> GetCardsByTitleContains(string title, bool caseSensitive)
        {
            var cmd = GetCommand();
            cmd.CommandText = Statements.Cards.SelectByTitleContains;
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@caseSensitive", caseSensitive);
            return ExecuteSelectCommand(cmd, Parse, true);
        }


        protected override void PreSave(Card card)
        {
            if (!card.CardType.Id.HasValue)
                CardTypeDataAccess.Save(card.CardType);

            if (card.PrimaryImage != null)
                ImageDataAccess.Save(card.PrimaryImage);
        }

        protected override void PostSave(Card card)
        {
            foreach (var link in card.HardLinks)
                HardLinkDataAccess.Save(link);

            foreach (var link in card.ReferencedBy)
                HardLinkDataAccess.Save(link);

            foreach (var image in card.AdditionalImages)
                ImageDataAccess.SaveImageToCard(image, card);
        }

        protected override void ValidateSave(Card card)
        {
            if (card.CardType == null)
                throw new InvalidOperationException("Card must have a Card Type.");
        }

        protected override void AddParametersToInsertUpdateCommand(SQLiteCommand cmd, Card card)
        {
            cmd.Parameters.AddWithValue("@title", card.Title);
            cmd.Parameters.AddWithValue("@subtitle", card.Subtitle);
            cmd.Parameters.AddWithValue("@description", card.Description);
            cmd.Parameters.AddWithValue("@primaryImageId", card.PrimaryImage?.Id);
            cmd.Parameters.AddWithValue("@cardTypeId", card.CardType?.Id);
        }

        protected override Card Parse(IDataRecord record)
        {
            var id = record.ToInt("Id");
            var typeId = record.ToInt("CardTypeId");
            var imageId = record.ToNullableInt("PrimaryImageId");

            var card = new CardWrapper(CardTypeDataAccess, ImageDataAccess, HardLinkDataAccess, PropertyDataAccess, id, typeId, imageId);

            card.Id = id;
            card.Title = record.ToString("Title");
            card.Subtitle = record.ToString("Subtitle");
            card.Description = record.ToString("Description");
            card.RawText = record.ToString("RawText");
            
            return card;
        }
    }
}