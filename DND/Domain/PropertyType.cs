using System.Collections.Generic;

namespace DND.Domain
{
    public class PropertyType : DomainObject, INamed
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}