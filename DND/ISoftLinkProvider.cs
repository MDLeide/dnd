using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DND.Domain;

namespace DND
{
    public interface ISoftLinkProvider
    {
        IEnumerable<SoftLink> GetSoftLinks(Card card);
        IEnumerable<SoftLink> GetSoftLinks(string rawText);
    }
}
