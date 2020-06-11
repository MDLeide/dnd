using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DND.Domain;

namespace DND.Data
{
    public class DomainCollection<T>
        where T : DomainObject
    {
        public void DiscardChanges()
        {

        }

        public void AcceptChanges()
        {

        }
    }
}
