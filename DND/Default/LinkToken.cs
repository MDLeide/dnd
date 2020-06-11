using System;
using System.Linq;
using DND.Parser;

namespace DND.Default
{
    public class LinkToken : Token
    {
        const char Delimiter = '`';

        LinkToken(TokenType linkTokenType, string rawValue, string value, int position, int id, bool hasId, bool isOpen)
            : base(linkTokenType, rawValue, value, position, isOpen)
        {
            Id = id;
            HasId = hasId;
        }

        public static LinkToken Create(TokenType linkTokenType, string rawValue, string value, int position, bool isOpen)
        {
            int cnt = rawValue.Count(p => p == Delimiter);

            if (cnt > 1)
                throw new ArgumentException($"Raw Value must contain 1 or 0 {Delimiter} characters.", nameof(rawValue));

            if (cnt == 1)
            {
                int pos = rawValue.IndexOf(Delimiter);
                if (!int.TryParse(rawValue.Substring(0, pos), out int id))
                    throw new ArgumentException($"RawValue must start with an int if it contains a '{Delimiter}' character");

                return new LinkToken(linkTokenType, rawValue, value, position, id, true, isOpen);
            }

            return new LinkToken(linkTokenType, rawValue, value, position, -1, false, isOpen);
        }

        public int Id { get; }
        public bool HasId { get; }
    }
}