using System;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using DND.Data;
using DND.Default;
using DND.Domain;
using DND.Parser;
using DND.UI.Components;
using DND.UI.Screens.Select;
using DND.UI.Util;

namespace DND.UI.Domain
{
    public class CardViewModel : DomainViewModel<DND.Domain.Card>
    {
        public CardViewModel(DND.Domain.Card card)
            : base(card, DataAccessManager.CardDataAccess)
        {
            CardType = new CardTypeViewModel(card.CardType);
            LinkedText = Util.LinkedText.GetLinkedTextViewModel();
            LinkedText.Text = card.Description;
        }

        public DND.Domain.Card Card => Object;

        public string Title
        {
            get => Object.Title;
            set
            {
                Object.Title = value;
                NotifyOfPropertyChange(nameof(Title));
            }
        }

        public string Description
        {
            get => Object.Description;
            set
            {
                Object.Description = value;
                NotifyOfPropertyChange(nameof(Description));
                LinkedText.Text = value;
            }
        }

        public string Subtitle
        {
            get => Object.Subtitle;
            set
            {
                Object.Subtitle = value;
                NotifyOfPropertyChange(nameof(Subtitle));
            }
        }

        public string RawText
        {
            get => Object.RawText;
            set
            {
                Object.RawText = value;
                NotifyOfPropertyChange(nameof(RawText));
            }
        }

        LinkedTextViewModel _linkedText;
        public LinkedTextViewModel LinkedText
        {
            get => _linkedText;
            set
            {
                if (Equals(value, _linkedText)) return;
                _linkedText = value;
                NotifyOfPropertyChange(nameof(LinkedText));
            }
        }

        CardTypeViewModel _cardType;
        public CardTypeViewModel CardType
        {
            get => _cardType ?? (_cardType = new CardTypeViewModel(Card.CardType));
            set
            {
                if (Equals(value, _cardType)) return;
                _cardType = value;
                if (value != null)
                    Card.CardType = value.CardType;
                NotifyOfPropertyChange(nameof(CardType));
            }
        }

        ImageViewModel _primaryImage;
        public ImageViewModel PrimaryImage
        {
            get => _primaryImage ?? (_primaryImage = new ImageViewModel(Card.PrimaryImage ?? Images.DefaultImage));
            set
            {
                if (Equals(value, _primaryImage)) return;
                _primaryImage = value;
                NotifyOfPropertyChange(nameof(PrimaryImage));
            }
        }

        ObservableCollection<ImageViewModel> _additionalImages;
        public ObservableCollection<ImageViewModel> AdditionalImages
        {
            get
            {
                if (_additionalImages == null)
                {
                    _additionalImages =
                        new ObservableCollection<ImageViewModel>(
                            Card.AdditionalImages.Select(p => new ImageViewModel(p)));
                    CollectionSynch.Link(_additionalImages, Card.AdditionalImages);
                }

                return _additionalImages;
            }
            set
            {
                if (Equals(value, _additionalImages)) return;
                _additionalImages = value;
                NotifyOfPropertyChange(nameof(AdditionalImages));
            }
        }

        ObservableCollection<SoftLinkViewModel> _softLinks;
        public ObservableCollection<SoftLinkViewModel> SoftLinks
        {
            get
            {
                if (_softLinks == null)
                {
                    _softLinks = new ObservableCollection<SoftLinkViewModel>(
                        Card.SoftLinks.Select(p => new SoftLinkViewModel(p)));
                    CollectionSynch.Link(_softLinks, Card.SoftLinks, t => t.SoftLink);
                }

                return _softLinks;
            }
            set
            {
                if (Equals(value, _softLinks)) return;
                _softLinks = value;
                NotifyOfPropertyChange(nameof(SoftLinks));
            }
        }


        public void ChangeImage()
        {
            var vm = new SelectorViewModel<DND.Domain.Image, ImageViewModel>(
                DataAccessManager.ImageDataAccess.Get().Select(p => new ImageViewModel(p)).ToArray,
                PrimaryImage);

            if (ShowDialog(vm))
            {
                Card.PrimaryImage = vm.SelectedItem?.Image;
                PrimaryImage = vm.SelectedItem;
            }
        }

        public void ChangeCardType()
        {
            var vm = new SelectorViewModel<DND.Domain.CardType, CardTypeViewModel>(
                DataAccessManager.CardTypeDataAccess.Get().Select(p => new CardTypeViewModel(p)).ToArray(),
                CardType);

            if (ShowDialog(vm))
            {
                Card.CardType = vm.SelectedItem?.CardType;
                CardType = vm.SelectedItem;
            }
        }
    }
}