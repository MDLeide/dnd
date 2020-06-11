using System;
using DND.UI.Domain;

namespace DND.UI.Components
{
    public class CardOpenedEventArgs : EventArgs
    {
        public CardOpenedEventArgs(CardViewModel cardOpened)
        {
            CardOpened = cardOpened;
        }

        public CardViewModel CardOpened { get; }
    }
}