using DND.DataAccess;
using DND.Domain;

namespace DND.Data
{
    public class CardProvider : ICardProvider
    {
        public CardProvider(ICardDataAccess cardDataAccess)
        {
            DataAccess = cardDataAccess;
        }

        ICardDataAccess DataAccess { get; }

        public Card Get(string title)
        {
            return DataAccess?.GetCardByTitle(title);
        }
    }
}