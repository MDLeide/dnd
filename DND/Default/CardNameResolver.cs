using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DND.Parser;

namespace DND.Default
{
    public class CardNameResolver : ICardNameResolver
    {
        public string GetCardName(Token token)
        {
            return token.Value;
        }
    }
}
