namespace DND.Parser
{
    public class TokenType
    {
        public TokenType(string name, string openSymbol, string escapeOpen, string closeSymbol, string escapeClose)
        {
            Name = name;
            OpenSymbol = openSymbol;
            EscapeOpen = escapeOpen;
            CloseSymbol = closeSymbol;
            EscapeClose = escapeClose;
        }

        public string Name { get; }
        public string OpenSymbol { get; }
        public string EscapeOpen { get; }
        public string CloseSymbol { get; }
        public string EscapeClose { get; }

        public Token GetToken(string rawValue, string value, int position, bool isOpen = false)
        {
            return CreateToken(rawValue, value, position, isOpen);
        }

        protected virtual Token CreateToken(string rawValue, string value, int position, bool isOpen)
        {
            return new Token(this, rawValue, value, position, isOpen);
        }
    }
}