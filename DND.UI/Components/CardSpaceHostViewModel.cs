using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cashew.UI.WPF.MVVM;
using DND.UI.Domain;

namespace DND.UI.Components
{
    class CardSpaceHostViewModel : Screen
    {
        public CardSpaceHostViewModel(CardSpaceViewModel cardSpace)
        {
            CardSpace = cardSpace;
            CardSearch = new CardSearchViewModel();
            CardSearch.CardOpened += (s, e) => CardSpace.Open(e.CardOpened);
        }

        CardSearchViewModel _cardSearch;
        public CardSearchViewModel CardSearch
        {
            get => _cardSearch;
            set
            {
                if (Equals(value, _cardSearch)) return;
                _cardSearch = value;
                NotifyOfPropertyChange(nameof(CardSearch));
            }
        }


        CardSpaceViewModel _cardSpace;
        public CardSpaceViewModel CardSpace
        {
            get => _cardSpace;
            set
            {
                if (Equals(value, _cardSpace)) return;
                _cardSpace = value;
                NotifyOfPropertyChange(nameof(CardSpace));
            }
        }
    }
}
