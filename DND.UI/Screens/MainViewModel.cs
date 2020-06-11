using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Cashew.UI;
using Cashew.UI.WPF.MVVM;
using DND.UI.Design;
using DND.UI.Util;

namespace DND.UI.Screens
{
    class MainViewModel : Screen
    {
        public MainViewModel()
        {
            ScreenStack.TopScreenChanged += ScreenStackOnTopScreenChanged;
            Menu = new MainMenuViewModel();
            ScreenStack.PushOrFloat(new CardTableViewModel());
        }

        void ScreenStackOnTopScreenChanged(object sender, ScreenChangedEventArgs e)
        {
            CurrentViewModel = e.NewScreen;
        }

        Screen _currentViewModel;
        public Screen CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (Equals(value, _currentViewModel)) return;
                _currentViewModel = value;
                NotifyOfPropertyChange(nameof(CurrentViewModel));
            }
        }

        MainMenuViewModel _menu;
        public MainMenuViewModel Menu
        {
            get => _menu;
            set
            {
                if (Equals(value, _menu)) return;
                _menu = value;
                NotifyOfPropertyChange(nameof(Menu));
            }
        }

        #region Test

        RelayCommand _test;

        public RelayCommand Test
        {
            get { return _test ?? (_test = new RelayCommand(o => OnTest(), o => CanTest())); }
        }

        void OnTest()
        {
            var x = LorumIpsum.RandomTinySentence; 
            x = LorumIpsum.RandomShortSentence;
            x = LorumIpsum.RandomAverageSentence;
            x = LorumIpsum.RandomLongSentence;
            x = LorumIpsum.RandomHugeSentence;

            x = LorumIpsum.RandomShortParagraph;
            x = LorumIpsum.RandomAverageParagraph;
            x = LorumIpsum.RandomLongParagraph;

            x = LorumIpsum.RandomPassage;
        }

        bool CanTest()
        {
            return true;
        }

        #endregion Test
    }
}
