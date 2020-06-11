using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DND.DataAccess;
using DND.Domain;

namespace DND.UI.Domain
{
    public class CardSpaceCardViewModel : DomainViewModel<DND.Domain.CardSpaceCard>
    {
        public CardSpaceCardViewModel(DND.Domain.CardSpaceCard cardSpaceCard)
            : base(cardSpaceCard, DataAccessManager.CardSpaceCardDataAccess)
        {
            CardSpaceCard = cardSpaceCard;
        }

        public DND.Domain.CardSpaceCard CardSpaceCard { get; }

        CardViewModel _card;
        public CardViewModel Card
        {
            get => _card ?? (_card = new CardViewModel(CardSpaceCard.Card));
            set
            {
                if (Equals(value, _card)) return;
                _card = value;
                NotifyOfPropertyChange(nameof(Card));
            }
        }

        public double PositionLeft
        {
            get => Object.PositionLeft;
            set
            {
                Object.PositionLeft = value;
                NotifyOfPropertyChange(nameof(PositionLeft));
            }
        }

        public double PositionTop
        {
            get => Object.PositionTop;
            set
            {
                Object.PositionTop = value;
                NotifyOfPropertyChange(nameof(PositionTop));
            }
        }

        public int ZIndex
        {
            get => Object.ZIndex;
            set
            {
                Object.ZIndex = value;
                NotifyOfPropertyChange(nameof(ZIndex));
            }
        }
        
        public CardVisualState VisualState
        {
            get => Object.VisualState;
            set
            {
                Object.VisualState = value;
                NotifyOfPropertyChange(nameof(VisualState));
            }
        }
    }
}
