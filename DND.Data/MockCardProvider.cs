using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DND.Domain;

namespace DND.Data
{
    public class MockCardProvider : ICardProvider
    {
        readonly Dictionary<string, Card> _cards = new Dictionary<string, Card>();

        public Card Get(string title)
        {
            if (!_cards.ContainsKey(title))
                _cards.Add(title, new Card(){Title = title});
            return _cards[title];
        }
    }
}
