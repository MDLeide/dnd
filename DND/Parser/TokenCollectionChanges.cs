using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DND.Parser
{
    public class TokenCollectionChanges
    {
        public TokenCollectionChanges(
            IEnumerable<Token> removed, 
            IEnumerable<Token> added,
            IEnumerable<Token> positionChanged,
            bool inFront)
        {
            Removed = new ReadOnlyCollection<Token>(removed.ToList());
            Added = new ReadOnlyCollection<Token>(added.ToList());
            PositionChanged = new ReadOnlyCollection<Token>(positionChanged.ToList());
            InFront = inFront;
        }

        public ReadOnlyCollection<Token> Removed { get; }
        public ReadOnlyCollection<Token> Added { get; }
        public ReadOnlyCollection<Token> PositionChanged { get; }
        public bool InFront { get; }
    }
}