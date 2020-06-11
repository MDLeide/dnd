using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cashew.UI.WPF.MVVM;
using DND.UI.Components;

namespace DND.UI.Screens
{
    class CardTableViewModel : Screen
    {
        public CardTableViewModel()
        {
            //CardSearch = new CardSearchViewModel();
            //CardSearch.CardOpened += SearchOnCardOpened;
            //CardSpace = new CardSpaceViewModel();
        }

        //CardSpaceViewModel _cardSpace;
        //public CardSpaceViewModel CardSpace
        //{
        //    get => _cardSpace;
        //    set
        //    {
        //        if (Equals(value, _cardSpace)) return;
        //        _cardSpace = value;
        //        NotifyOfPropertyChange(nameof(CardSpace));
        //    }
        //}

        //CardSearchViewModel _cardSearch;
        //public CardSearchViewModel CardSearch
        //{
        //    get => _cardSearch;
        //    set
        //    {
        //        if (Equals(value, _cardSearch)) return;
        //        _cardSearch = value;
        //        NotifyOfPropertyChange(nameof(CardSearch));
        //    }
        //}

        //void SearchOnCardOpened(object sender, CardOpenedEventArgs e)
        //{
        //    var existing = CardSpace.Cards.FirstOrDefault(p => p.Card.Id == e.CardOpened.Card.Id);
        //    if (existing == null)
        //    {
        //        existing = CardSearch.SelectedCard;
        //        CardSpace.Cards.Add(CardSearch.SelectedCard);
        //    }

        //    CardSpace.SelectedCard = existing;
        //}
    }
}
