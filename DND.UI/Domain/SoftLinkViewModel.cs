using Cashew.UI.WPF.MVVM;
using DND.Domain;

namespace DND.UI.Domain
{
    public class SoftLinkViewModel : Screen
    {
        public SoftLinkViewModel(SoftLink softLink)
        {
            SoftLink = softLink;
        }

        public SoftLink SoftLink { get; }

        public int Position
        {
            get => SoftLink.Position;
            set
            {
                SoftLink.Position = value;
                NotifyOfPropertyChange(nameof(Position));
            }
        }

        public CardViewModel Target
        {
            get => _target ?? (_target = new CardViewModel(SoftLink.Target));
            set
            {
                if (Equals(value, _target)) return;
                _target = value;
                NotifyOfPropertyChange(nameof(Target));
            }
        }

        CardViewModel _target;

        public string Text
        {
            get => SoftLink.Text;
            set
            {
                SoftLink.Text = value;
                NotifyOfPropertyChange(nameof(Text));
            }
        }
    }
}