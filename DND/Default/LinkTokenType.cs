using DND.Parser;

namespace DND.Default
{
    public class LinkTokenType : TokenType
    {
        public LinkTokenType()
            :base("link", "{", "{", "}", "}") { }

        protected override Token CreateToken(string rawValue, string value, int position, bool isOpen)
        {
            return LinkToken.Create(this, rawValue, value, position, isOpen);
        }
    }
}