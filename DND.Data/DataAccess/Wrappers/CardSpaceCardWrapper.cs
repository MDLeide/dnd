using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cashew.Data;
using DND.DataAccess;
using DND.Domain;

namespace DND.Data.DataAccess.Wrappers
{
    public class CardSpaceCardWrapper : CardSpaceCard
    {
        struct CardContext
        {
            public ICardDataAccess DataAccess { get; set; }
            public int CardId { get; set; }
        }

        struct CardSpaceContext
        {
            public ICardSpaceDataAccess DataAccess { get; set; }
            public int CardSpaceId { get; set; }
        }

        public CardSpaceCardWrapper(ICardDataAccess cardDataAccess,
            int cardId,
            ICardSpaceDataAccess cardSpaceDataAccess,
            int cardSpaceId)
        {
            var cardContext = new CardContext() {DataAccess = cardDataAccess, CardId = cardId};
            _card = new PropWrapper<Card>(
                cardContext,
                o => ((CardContext) o).DataAccess.Get(((CardContext) o).CardId));

            var spaceContext = new CardSpaceContext() {DataAccess = cardSpaceDataAccess, CardSpaceId = cardSpaceId};
            _cardSpace = new PropWrapper<CardSpace>(
                spaceContext,
                o => ((CardSpaceContext) o).DataAccess.Get(((CardSpaceContext) o).CardSpaceId));
        }

        readonly PropWrapper<CardSpace> _cardSpace;

        public override CardSpace CardSpace
        {
            get => _cardSpace.Value;
            set => _cardSpace.Value = value;
        }

        readonly PropWrapper<Card> _card;

        public override Card Card
        {
            get => _card.Value;
            set => _card.Value = value;
        }
    }
}
