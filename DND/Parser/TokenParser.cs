using System;
using System.Collections.Generic;
using System.Linq;
using DND.Domain;

namespace DND.Parser
{
    public class TokenParser : ITokenParser
    {
        public TokenParser(IEnumerable<TokenType> supportedTypes)
        {
            SupportedTypes = supportedTypes.ToArray();
            LongestSymbol = SupportedTypes.Select(p =>
                    Math.Max(
                        p.EscapeOpen.Length + p.OpenSymbol.Length,
                        p.EscapeClose.Length + p.CloseSymbol.Length))
                .Max();
        }


        public IEnumerable<TokenType> SupportedTypes { get; }

        public int LongestSymbol { get; }


        public IEnumerable<Token> Parse(string input) => Parse(input, SupportedTypes);

        public static IEnumerable<Token> Parse(string input, IEnumerable<TokenType> types)
        {
            var position = 0;
            Token t;
            var tokens = new List<Token>();
            while ((t = FindNextToken(input, position, types)) != null)
            {
                tokens.Add(t);
                position = t.Position + t.RawValue.Length;
            }
            return tokens;
        }

        public string EscapeString(string input) => EscapeString(input, SupportedTypes);


        public static string GetValue(TokenType type, string rawValue)
        {
            if (!rawValue.StartsWith(type.OpenSymbol))
                throw new InvalidOperationException($"Not a valid value for token type {type.Name}: raw value must begin with {type.OpenSymbol}");
            if (!rawValue.EndsWith(type.CloseSymbol))
                throw new InvalidOperationException($"Not a valid value for token type {type.Name}: raw value must end with {type.CloseSymbol}");

            return EscapeString(rawValue, type).Substring(
                type.OpenSymbol.Length,
                rawValue.Length - type.OpenSymbol.Length - type.CloseSymbol.Length);
        }
        
        public static string EscapeString(string input, TokenType type) => EscapeString(input, new[] { type });

        public static string EscapeString(string input, IEnumerable<TokenType> types)
        {
            foreach (var t in types)
                input =
                    input.Replace(t.EscapeOpen + t.OpenSymbol, t.OpenSymbol)
                        .Replace(t.EscapeClose + t.CloseSymbol, t.CloseSymbol);
            return input;
        }

        // several problems here. consider two types with opening symbol { and {{
        // need to lookahead at other types
        static Token FindNextToken(string input, int start, IEnumerable<TokenType> types)
        {
            TokenType opened = null;
            int position = 0;

            for (int i = start; i < input.Length; i++)
            {
                if (opened != null)
                {
                    if (i + opened.CloseSymbol.Length > input.Length)
                        return opened.GetToken(
                            input.Substring(position, input.Length - position),
                            input.Substring(position, input.Length - position),
                            position,
                            true);

                    if (SymbolMatches(input, i, opened.CloseSymbol, opened.EscapeClose))
                        return opened.GetToken(
                            input.Substring(position, i - position + 1),
                            GetValue(opened, input.Substring(position, i - position + 1)),
                            position);
                }
                else
                {
                    foreach (var t in types)
                    {
                        if (i + t.OpenSymbol.Length > input.Length)
                            continue;

                        if (SymbolMatches(input, i, t.OpenSymbol, t.EscapeOpen))
                        {
                            opened = t;
                            position = i;
                            break;
                        }
                    }
                }
            }

            if (opened == null)
                return null;

            return opened.GetToken(
                input.Substring(position, input.Length - position),
                input.Substring(position, input.Length - position),
                position,
                true);
        }

        static bool SymbolMatches(string input, int position, string symbol, string escapeSymbol)
        {
            if (position + symbol.Length > input.Length)
                return false;

            if (input.Substring(position, symbol.Length) != symbol)
                return false;
            
            // is escaped
            if (IsBehind(input, position, escapeSymbol))
                return false;

            // is escaping
            if (IsAhead(input, position, escapeSymbol) && IsAhead(input, position + escapeSymbol.Length, symbol))
                return false;

            return true;
        }

        static bool IsBehind(string input, int position, string val)
        {
            return SubstringMatches(input, position - val.Length, val);
        }

        static bool IsAhead(string input, int position, string val)
        {
            return SubstringMatches(input, position, val);
        }

        static bool SubstringMatches(string input, int position, string val)
        {
            if (position < 0 || position >= input.Length)
                return false;

            if (position + val.Length > input.Length)
                return false;

            return input.Substring(position, val.Length) == val;
        }

        IEnumerable<Token> ITokenParser.Parse(string input, IEnumerable<TokenType> types) => Parse(input, types);
        string ITokenParser.GetValue(TokenType type, string rawValue) => GetValue(type, rawValue);
        string ITokenParser.EscapeString(string input, TokenType type) => EscapeString(input, type);
        string ITokenParser.EscapeString(string input, IEnumerable<TokenType> possibleTypes) => EscapeString(input, possibleTypes);
    }
}