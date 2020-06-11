using System.Collections.Generic;

namespace DND.Domain
{
    public class CardType : DomainObject, INamed
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual Image Image { get; set; }
        public virtual List<PropertyType> PropertyTypes { get; set; } = new List<PropertyType>();
    }
}