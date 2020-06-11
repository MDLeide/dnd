using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DND.Default;

namespace DND.Parser
{
    public class LiveParser : ILiveParser
    {
        readonly ITokenParser _parser;
        readonly List<Token> _tokens = new List<Token>();


        public LiveParser(ITokenParser parser)
        {
            _parser = parser;
            Tokens = new ReadOnlyCollection<Token>(_tokens);
        }

        public string Text { get; private set; }

        public TokenCollectionChanges SetText(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                var args =
                    new TokenCollectionChanges(_tokens.ToArray(), new Token[] { }, new Token[] { }, true);

                Text = string.Empty;
                _tokens.Clear();
                FirstToken = null;
                LastToken = null;
                return args;
            }

            var oldValue = Text;
            Text = val;
            return Update(oldValue, val);
        }

        public ReadOnlyCollection<Token> Tokens { get; }
        public Token FirstToken { get; private set; }
        public Token LastToken { get; private set; }
        public bool IsOpen => LastToken?.IsOpen ?? false;

        TokenCollectionChanges Update(string oldValue, string newValue)
        {
            GetDetails(oldValue, newValue, out string toParse, out bool inFront, out int position);
            RemoveAndAdjustPosition(position, inFront, out IEnumerable<Token> removed, out IEnumerable<Token> positionChanged);

            IEnumerable<Token> newTokens = string.IsNullOrEmpty(toParse) ? new Token[] { } : _parser.Parse(toParse).ToArray();

            InsertAndAdjustPosition(newTokens, inFront, position);

            FirstToken = _tokens.FirstOrDefault();
            LastToken = _tokens.LastOrDefault();

            return new TokenCollectionChanges(removed.ToArray(), newTokens.ToArray(),  positionChanged.ToArray(), inFront);
        }

        void InsertAndAdjustPosition(IEnumerable<Token> newTokens, bool inFront, int position)
        {
            foreach (var t in newTokens)
            {
                if (inFront)
                {
                    _tokens.Insert(0, t);
                }
                else
                {
                    t.Position += position;
                    _tokens.Add(t);
                }
            }
        }

        void GetDetails(string oldValue, string newValue, out string toParse, out bool inFront, out int position)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                position = 0;
                toParse = newValue;
                inFront = false;
            }
            else if (newValue.EndsWith(oldValue)) // typing at the start of the string
            {
                if (FirstToken == null)
                {
                    toParse = newValue;
                    position = newValue.Length - 1;
                }
                else
                {
                    toParse = newValue.Substring(0, FirstToken.Position);
                    position = FirstToken.Position - 1;
                }

                inFront = true;
            }
            else if (oldValue.EndsWith(newValue)) // deleting from start of string
            {
                position = oldValue.Length - newValue.Length;
                toParse = newValue.Substring(0, position);
                inFront = true;
            }
            else if (newValue.StartsWith(oldValue)) // typing at the end of the string
            {
                if (LastToken != null)
                {
                    if (LastToken.IsOpen) // open token at end of old string
                        position = LastToken.Position;
                    else
                        position = Math.Max(
                            LastToken.Position + LastToken.Length,
                            newValue.Length - _parser.LongestSymbol);
                }
                else
                {
                    position = newValue.Length - _parser.LongestSymbol;
                }

                toParse = newValue.Substring(position);
                inFront = false;
            }
            else if (oldValue.StartsWith(newValue)) // deleting from end of string
            {
                if (LastToken != null
                    && LastToken.Position < newValue.Length
                    && (LastToken.IsOpen ||
                        LastToken.Position + LastToken.Length > newValue.Length))
                    position = LastToken.Position;
                else
                    position = newValue.Length - _parser.LongestSymbol;

                toParse = newValue.Substring(position);
                inFront = false;
            }
            else // inserting into middle or deleting/replacing everything
            {
                position = 0;
                toParse = newValue;
                inFront = false;
            }
        }
        
        void RemoveAndAdjustPosition(int position, bool inFront, out IEnumerable<Token> removed, out IEnumerable<Token> positionChanged)
        {
            var removeList = new List<Token>();
            var positionList = new List<Token>();

            foreach (var token in Tokens)
            {
                if (inFront)
                {
                    if (token.Position < position)
                        removeList.Add(token);
                    else
                    {
                        token.Position -= position;
                        positionList.Add(token);
                    }
                }
                else
                {
                    if (token.Position >= position)
                        removeList.Add(token);
                }
            }

            foreach (var t in removeList)
                _tokens.Remove(t);

            removed = removeList;
            positionChanged = positionList;
        }
    }
}