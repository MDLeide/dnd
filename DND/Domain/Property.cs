using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND.Domain
{
    public class Property : DomainObject
    {
        public virtual Card Owner { get; set; }
        public virtual PropertyType PropertyType { get; set; }
        public virtual string Value { get; set; }
    }
}
