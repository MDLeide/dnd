using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DND.Domain;
using DND.Parser;

namespace DND.Default
{
    public class SoftLinkProvider : ISoftLinkProvider
    {
        readonly ITokenParser _tokenParser;
        readonly ICardLinker _cardLinker;

        public SoftLinkProvider(ITokenParser tokenParser, ICardLinker cardLinker)
        {
            _tokenParser = tokenParser;
            _cardLinker = cardLinker;
        }

        public IEnumerable<SoftLink> GetSoftLinks(Card card) => GetSoftLinks(card.RawText);

        public IEnumerable<SoftLink> GetSoftLinks(string rawText)
        {
            var tokens = _tokenParser.Parse(rawText);
            var links = new List<SoftLink>();

            foreach (var t in tokens.Where(p => _cardLinker.CanLink(p)))
            {
                var link = _cardLinker.Link(t);
                links.Add(link);
            }

            return links;
        }
    }
}
