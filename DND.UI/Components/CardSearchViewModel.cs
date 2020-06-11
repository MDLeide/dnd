using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cashew.UI.WPF.MVVM;
using DND.DataAccess;
using DND.Domain;
using DND.UI.Domain;

namespace DND.UI.Components
{
    public class CardSearchViewModel : Screen
    {
        string _cardTitlePartLastUsed = string.Empty;
        IEnumerable<CardViewModel> _searchResults = new CardViewModel[] { };


        public event EventHandler<CardOpenedEventArgs> CardOpened;


        public ICardDataAccess DataAccess { get; } = DataAccessManager.CardDataAccess;

        string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (Equals(value, _searchTerm)) return;
                _searchTerm = value;
                NotifyOfPropertyChange(nameof(SearchTerm));
                Update(value);
            }
        }

        string _cardTypePart;
        public string CardTypePart
        {
            get => _cardTypePart;
            set
            {
                if (Equals(value, _cardTypePart)) return;
                _cardTypePart = value;
                NotifyOfPropertyChange(nameof(CardTypePart));
            }
        }

        string _cardTitlePart;
        public string CardTitlePart
        {
            get => _cardTitlePart;
            set
            {
                if (Equals(value, _cardTitlePart)) return;
                _cardTitlePart = value;
                NotifyOfPropertyChange(nameof(CardTitlePart));
            }
        }

        ObservableCollection<CardViewModel> _cards = new ObservableCollection<CardViewModel>();
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

                if (OpenSelectedCardAutomatically)
                    Open();
            }
        }

        bool _caseSensitive;
        public bool CaseSensitive
        {
            get => _caseSensitive;
            set
            {
                if (Equals(value, _caseSensitive)) return;
                _caseSensitive = value;
                NotifyOfPropertyChange(nameof(CaseSensitive));
            }
        }

        bool _startsWith;
        public bool StartsWith
        {
            get => _startsWith;
            set
            {
                if (Equals(value, _startsWith)) return;
                _startsWith = value;
                NotifyOfPropertyChange(nameof(StartsWith));
            }
        }

        bool _openSelectedCardAutomatically;
        public bool OpenSelectedCardAutomatically
        {
            get => _openSelectedCardAutomatically;
            set
            {
                if (Equals(value, _openSelectedCardAutomatically)) return;
                _openSelectedCardAutomatically = value;
                NotifyOfPropertyChange(nameof(OpenSelectedCardAutomatically));
            }
        }

        public void Open()
        {
            if (SelectedCard == null)
                return;
            SearchTerm = SelectedCard.Title;
            CardOpened?.Invoke(this, new CardOpenedEventArgs(SelectedCard));
        }

        void Update(string term)
        {
            UpdateCardTitlePart(term, StartsWith, CaseSensitive);
        }

        void UpdateCardTitlePart(string titlePart, bool startsWith, bool caseSensitive)
        {
            if (!caseSensitive)
                titlePart = titlePart.ToLower();

            if (!string.IsNullOrEmpty(_cardTitlePartLastUsed) && titlePart.Contains(caseSensitive ? _cardTitlePartLastUsed : _cardTitlePartLastUsed.ToLower()))
            {
                var results = _searchResults.Where(p =>
                {
                    var title = caseSensitive ? p.Title : p.Title.ToLower();
                    return startsWith ? title.StartsWith(titlePart) : title.Contains(titlePart);
                });
                Cards = new ObservableCollection<CardViewModel>(results);
            }
            else
            {
                _searchResults = (startsWith
                    ? DataAccess.GetCardsByTitleStartsWith(titlePart, caseSensitive)
                    : DataAccess.GetCardsByTitleContains(titlePart, caseSensitive)).Select(p => new CardViewModel(p)).ToArray();
                _cardTitlePartLastUsed = titlePart;
                Cards = new ObservableCollection<CardViewModel>(_searchResults);
            }
        }

        void UpdateCardTypePart(string typePart)
        {

        }
    }
}
