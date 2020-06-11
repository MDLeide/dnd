using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cashew.UI;
using Cashew.UI.WPF.MVVM;
using DND.Domain;
using DND.UI.Components;
using DND.UI.Domain;

namespace DND.UI.Design
{
    class DesignCardSearchViewModel
    {

    }

    class DesignCardSpaceViewModel
    {
        public DesignCardSpaceViewModel()
        {
            Cards = new ObservableCollection<DesignCardViewModel>();

            for (int i = 0; i < 6; i++)
                Cards.Add(new DesignCardViewModel());

            SelectedCard = Cards.FirstOrDefault();
        }

        public string Name { get; set; } = LorumIpsum.RandomTitle;

        public string Description { get; set; } = LorumIpsum.RandomShortParagraph;

        public ObservableCollection<DesignCardViewModel> Cards { get; set; }

        public DesignCardSearchViewModel CardSearch { get; set; }

        public DesignCardViewModel SelectedCard { get; set; }

        public DesignImageViewModel BackgroundImage { get; set; } = new DesignImageViewModel();

        public string BackgroundColorCode { get; set; } = "#AA00EE";

        #region Open

        RelayCommand _open;
        public RelayCommand Open => _open ?? (_open = new RelayCommand(OnOpen, CanOpen));

        void OnOpen(object param)
        {
        }

        bool CanOpen(object param) => true;

        #endregion Open
    }

    class DesignCardSpaceCardViewModel
    {
        static int _zIndex;
        static readonly Random Rand = new Random();

        public DesignCardSpaceCardViewModel()
        {
            ZIndex = ++_zIndex;
            PositionLeft = Rand.NextDouble() * Rand.Next(1, 300);
            PositionTop = Rand.NextDouble() * Rand.Next(1, 300);
        }

        public DesignCardViewModel Card { get; set; } = new DesignCardViewModel();
        public double PositionLeft { get; set; }
        public double PositionTop { get; set; }
        public int ZIndex { get; set; }
    }
}
