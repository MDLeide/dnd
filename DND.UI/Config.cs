using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DND.Parser;

namespace DND.UI
{
    static class Config
    {
        public static ILiveParser LiveParser { get; set; }
        public static ICardLinker CardLinker { get; set; }
    }
}
