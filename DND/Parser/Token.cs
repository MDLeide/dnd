namespace DND.Parser
{
    public class Token
    {
        public Token(TokenType type, string rawValue, string value, int position, bool isOpen = false)
        {
            Type = type;
            RawValue = rawValue;
            Value = value;
            Position = position;
            IsOpen = isOpen;
        }
        
        public TokenType Type { get; }
        public bool IsOpen { get; }
        public string Value { get; }
        public string RawValue { get; }
        public int Position { get; set; }
        public int Length => RawValue?.Length ?? 0;
    }
}
