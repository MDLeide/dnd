using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Cashew.UI.WPF.MVVM;
using DND.Domain;

namespace DND.UI.Domain
{
    public class CardSpaceViewModel : DomainViewModel<DND.Domain.CardSpace>
    {
        public CardSpaceViewModel(DND.Domain.CardSpace cardSpace)
            : base(cardSpace, DataAccessManager.CardSpaceDataAccess)
        {
            CardSpace = cardSpace;
            Cards = new ObservableCollection<CardSpaceCardViewModel>(
                cardSpace.Cards.Select(p => new CardSpaceCardViewModel(p)));
        }

        public DND.Domain.CardSpace CardSpace { get; }

        #region Open
        RelayCommand _open;

        public RelayCommand OpenCommand => _open ?? (_open = new RelayCommand(OnOpen, CanOpen));

        public void Open(CardViewModel card)
        {
            var existing = Cards.FirstOrDefault(p => p.Card.Id == card.Id);
            if (existing != null)
            {
                OpenImpl(existing);
                return;
            }

            var cardSpaceCard = new DND.Domain.CardSpaceCard();
            cardSpaceCard.Card = card.Card;
            cardSpaceCard.CardSpace = CardSpace;
            OpenImpl(new CardSpaceCardViewModel(cardSpaceCard));
        }

        public void Open(CardSpaceCardViewModel cardSpaceCard)
        {
            var existing = Cards.FirstOrDefault(p => p.Id == cardSpaceCard.Id);
            if (existing != null)
            {
                SelectedCard = existing;
                return;
            }

            OpenImpl(cardSpaceCard);
        }

        void OpenImpl(CardSpaceCardViewModel cardSpaceCard)
        {
            Cards.Add(cardSpaceCard);
            SelectedCard = cardSpaceCard;
        }

        void OnOpen(object obj)
        {
            if (obj is FrameworkElement fe)
                obj = fe.DataContext;

            if (obj is CardSpaceCardViewModel vm)
            {
                var existing = Cards.FirstOrDefault(p => p.Id == vm.Id);
                if (existing == null)
                {
                    existing = vm;
                    Cards.Add(existing);
                }

                if (Settings.CardSpaceSettings.SetNewOpenedCardAsSelected)
                    SelectedCard = existing;
            }
        }

        bool CanOpen(object obj) => obj is CardSpaceCardViewModel;
        #endregion Open

        public string Name
        {
            get => Object.Name;
            set
            {
                Object.Name = value;
                NotifyOfPropertyChange(nameof(Name));
            }
        }

        public string Description
        {
            get => Object.Description;
            set
            {
                Object.Description = value;
                NotifyOfPropertyChange(nameof(Description));
            }
        }

        public string BackgroundColorCode
        {
            get => Object.BackgroundColorCode;
            set
            {
                Object.BackgroundColorCode = value;
                NotifyOfPropertyChange(nameof(BackgroundColorCode));
            }
        }

        ImageViewModel _backgroundImage;
        public ImageViewModel BackgroundImage
        {
            get => _backgroundImage ?? (_backgroundImage = new ImageViewModel(CardSpace.BackgroundImage));
            set
            {
                if (Equals(value, _backgroundImage)) return;
                _backgroundImage = value;
                NotifyOfPropertyChange(nameof(BackgroundImage));
            }
        }

        ObservableCollection<CardSpaceCardViewModel> _cards;
        public ObservableCollection<CardSpaceCardViewModel> Cards
        {
            get => _cards;
            set
            {
                if (Equals(value, _cards)) return;
                _cards = value;
                NotifyOfPropertyChange(nameof(Cards));
            }
        }

        CardSpaceCardViewModel _selectedCard;
        public CardSpaceCardViewModel SelectedCard
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
