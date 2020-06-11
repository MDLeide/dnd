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
    public class CardSpaceCardsDataAccess : DataAccess<CardSpaceCard>, ICardSpaceCardDataAccess
    {
        public CardSpaceCardsDataAccess(ICardDataAccess cardDataAccess, ICardSpaceDataAccess cardSpaceDataAccess)
        {
            CardDataAccess = cardDataAccess;
            CardSpaceDataAccess = cardSpaceDataAccess;
        }

        public ICardDataAccess CardDataAccess { get; }
        public ICardSpaceDataAccess CardSpaceDataAccess { get; }

        protected override string InsertStatement => Statements.CardSpaceCards.Insert;
        protected override string UpdateStatement => Statements.CardSpaceCards.Update;
        protected override string DeleteStatement => Statements.CardSpaceCards.Delete;
        protected override string GetByIdStatement => null;
        protected override string GetAllStatement => null;

        protected override void ValidateSave(CardSpaceCard obj)
        {
            if (obj.Card?.Id == null)
                throw new InvalidOperationException("Must save Card before saving it to a Card Space.");

            if (obj.CardSpace?.Id == null)
                throw new InvalidOperationException("Must save Card Space before saving a Card to it.");
        }

        protected override CardSpaceCard Parse(IDataRecord record)
        {
            var cardId = record.ToInt("CardId");
            var spaceId = record.ToInt("SpaceId");

            var spaceCard = new CardSpaceCardWrapper(CardDataAccess, cardId, CardSpaceDataAccess, spaceId);
            spaceCard.Id = record.ToInt("Id");
            spaceCard.PositionLeft = record.ToDouble("PositionLeft");
            spaceCard.PositionTop = record.ToDouble("PositionTop");
            spaceCard.ZIndex = record.ToInt("ZIndex");
            return spaceCard;
        }

        protected override void AddParametersToInsertUpdateCommand(SQLiteCommand cmd, CardSpaceCard obj)
        {
            throw new NotImplementedException();
        }

        public CardSpaceCard GetByCardAndCardSpaceId(int cardId, int cardSpaceId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CardSpaceCard> GetByCardSpaceId(int cardSpaceId)
        {
            throw new NotImplementedException();
        }
    }
}