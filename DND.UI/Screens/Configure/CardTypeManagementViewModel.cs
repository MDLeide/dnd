using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cashew.UI.WPF.MVVM;
using Cashew.UI.WPF.MVVM.PropertyChange;
using DND.DataAccess;
using DND.Domain;
using DND.UI.Components.IO;
using DND.UI.Domain;
using DND.UI.Util;

namespace DND.UI.Screens.Configure
{
    public class CardTypeManagementViewModel : Screen
    {
        public ICardTypeDataAccess DataAccess { get; } = DataAccessManager.CardTypeDataAccess;

        CardTypeViewModel _selectedCardType;
        public CardTypeViewModel SelectedCardType
        {
            get => _selectedCardType;
            set
            {
                if (Equals(value, _selectedCardType)) return;
                _selectedCardType = value;
                NotifyOfPropertyChange(nameof(SelectedCardType));
            }
        }

        ObservableCollection<CardTypeViewModel> _cardTypes;
        public ObservableCollection<CardTypeViewModel> CardTypes
        {
            get => _cardTypes;
            set
            {
                if (Equals(value, _cardTypes)) return;
                _cardTypes = value;
                NotifyOfPropertyChange(nameof(CardTypes));
            }
        }
        
        public void NewCardType()
        {
            //todo: what if there is still an unsaved card?

            var type = new CardType();
            type.Name = "New Card Type";
            type.Description = "Description";
            type.Image = Images.DefaultCardTypeImage;
            var typeVm = new CardTypeViewModel(type);

            CardTypes.Add(typeVm);
            SelectedCardType = typeVm;
        }

        [NotifiedBy(nameof(SelectedCardType))]
        public bool CanSave => SelectedCardType != null;

        public void Save()
        {
            SelectedCardType.Save();
        }

        [NotifiedBy(nameof(SelectedCardType))]
        public bool CanDelete => SelectedCardType != null;

        public void Delete()
        {
            SelectedCardType.Delete();
            CardTypes.Remove(SelectedCardType);
            SelectedCardType = null;
        }

        protected override object OnLoad(object context)
        {
            var propTypes = DataAccessManager.PropertyTypeDataAccess.Get().ToArray();
            return DataAccess.Get().Select(p => new CardTypeViewModel(p, propTypes)).ToArray();
        } 
        
        protected override void PostLoad(object context, object loadContext) =>
            CardTypes = new ObservableCollection<CardTypeViewModel>((IEnumerable<CardTypeViewModel>) loadContext);
    }
}
