using System;

namespace DND.UI.Components.IO
{
    class InfoBarMessageChanged : EventArgs
    {
        public InfoBarMessageChanged(InfoBarEntry oldMessage, InfoBarEntry newMessage)
        {
            OldMessage = oldMessage;
            NewMessage = newMessage;
        }

        public InfoBarEntry OldMessage { get; }
        public InfoBarEntry NewMessage { get; }
    }
}