using System.Collections.Generic;

namespace DND.Domain
{
    public class Card : DomainObject, INamed
    {
        public string Name => Title;

        public virtual string Title { get; set; }
        public virtual string Subtitle { get; set; }
        public virtual string Description { get; set; }
        public virtual string RawText { get; set; }

        public virtual CardType CardType { get; set; }

        public virtual Image PrimaryImage { get; set; }
        public virtual List<Image> AdditionalImages { get; set; } = new List<Image>();

        public virtual List<SoftLink> SoftLinks { get; set; } = new List<SoftLink>();
        public virtual List<HardLink> HardLinks { get; set; } = new List<HardLink>();
        public virtual List<HardLink> ReferencedBy { get; set; } = new List<HardLink>();

        public virtual List<Property> Properties { get; set; } = new List<Property>();
    }
}