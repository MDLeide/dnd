using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cashew.UI.WPF.MVVM;
using DND.UI.Components;
using DND.UI.Domain;

namespace DND.UI.Screens
{
    public class AuthorViewModel : Screen
    {
        public AuthorViewModel()
        {
            CardSearch = new CardSearchViewModel();
            Cards = new ObservableCollection<CardViewModel>();
            CardSearch.CardOpened += (s, e) =>
            {
                var existing = Cards.FirstOrDefault(p => p.Id == e.CardOpened.Id);
                if (existing == null)
                {
                    existing = e.CardOpened;
                    Cards.Add(existing);
                }

                SelectedCard = existing;
            };
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

        ObservableCollection<CardViewModel> _cards;
        public ObservableCollection<CardViewModel> Cards
        {
            get => _cards;
            set
            {
                if (Equals(value, _cards)) return;
                _cards = value;
                NotifyOfPropertyChange(nameof(Cards));
            }
        }

        CardViewModel _selectedCard;
        public CardViewModel SelectedCard
        {
            get => _selectedCard;
            set
            {
                if (Equals(value, _selectedCard)) return;
                _selectedCard = value;
                NotifyOfPropertyChange(nameof(SelectedCard));
            }
        }
    }
}
