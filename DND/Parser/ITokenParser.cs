using System.Collections.Generic;

namespace DND.Parser
{
    public interface ITokenParser
    {
        IEnumerable<TokenType> SupportedTypes { get; }
        IEnumerable<Token> Parse(string input);
        IEnumerable<Token> Parse(string input, IEnumerable<TokenType> types);
        string GetValue(TokenType type, string rawValue);
        string EscapeString(string input);
        string EscapeString(string input, TokenType type);
        string EscapeString(string input, IEnumerable<TokenType> possibleTypes);
        int LongestSymbol { get; }
    }
}