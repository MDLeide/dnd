using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND.Domain
{
    public class CardSpace : DomainObject, INamed
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string BackgroundColorCode { get; set; }
        public virtual Image BackgroundImage { get; set; }
        public virtual List<CardSpaceCard> Cards { get; set; } = new List<CardSpaceCard>();
    }

    public class CardSpaceCard : DomainObject
    {
        public virtual Card Card { get; set; }
        public virtual CardSpace CardSpace { get; set; }
        public virtual double PositionLeft { get; set; }
        public virtual double PositionTop { get; set; }
        public virtual int ZIndex { get; set; }
        public virtual CardVisualState VisualState { get; set; }
    }

    public enum CardVisualState
    {
        Visible,
        Collapsed,
        ImageOnly,
        Minimized,
        Hidden
    }
}
