using System;
using Cashew.UI.WPF.MVVM;
using DND.Domain;
using DND.UI.Components;
using DND.UI.Domain;
using DND.UI.Screens.Configure;
using DND.UI.Util;

namespace DND.UI.Screens
{
    class MainMenuViewModel : Screen
    {
        public bool CanSave => false;

        public void Save()
        {

        }

        public void EditImages()
        {
            ScreenStack.PushOrFloat(new ImageManagementViewModel());
        }

        public void EditPropertyTypes()
        {
            ScreenStack.PushOrFloat(new PropertyTypeManagementViewModel());
        }

        public void EditCardTypes()
        {
            ScreenStack.PushOrFloat(new CardTypeManagementViewModel());
        }

        public void AuthorCards()
        {
            ScreenStack.PushOrFloat(new AuthorViewModel());
        }

        public void ShowCardSpace()
        {
            try
            {
                ScreenStack.PushOrFloat(new CardSpaceHostViewModel(new CardSpaceViewModel(new CardSpace())));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Settings()
        {

        }
    }
}