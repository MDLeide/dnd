using System;
using Cashew.UI.WPF.MVVM;

namespace DND.UI.Util
{
    class ScreenChangedEventArgs : EventArgs
    {
        public ScreenChangedEventArgs(Screen oldScreen, Screen newScreen)
        {
            OldScreen = oldScreen;
            NewScreen = newScreen;
        }

        public Screen OldScreen { get; }
        public Screen NewScreen { get; }
    }
}