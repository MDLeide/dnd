using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND.Domain
{
    public class Image : DomainObject
    {
        public string Location { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Size in bytes.
        /// </summary>
        public int Size { get; set; }
    }
}
