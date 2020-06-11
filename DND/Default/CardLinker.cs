using System;
using DND.Domain;
using DND.Parser;

namespace DND.Default
{
    public class CardLinker : ICardLinker
    {
        readonly ICardProvider _provider;
        readonly ICardNameResolver _cardNameResolver;

        public CardLinker(ICardProvider provider, ICardNameResolver cardNameResolver)
        {
            _provider = provider;
            _cardNameResolver = cardNameResolver;
        }

        public SoftLink Link(Token token)
        {
            var link = new SoftLinkWrapper(_provider, token, () => _cardNameResolver.GetCardName(token));
            link.Text = token.Value;
            link.Position = token.Position;
            return link;
        }

        public bool CanLink(Token token)
        {
            return token is LinkToken;
        }

        class SoftLinkWrapper : SoftLink
        {
            readonly ICardProvider _provider;
            readonly Token _token;
            readonly Func<string> _cardName;
            Card _target;

            public SoftLinkWrapper(ICardProvider provider, Token token, Func<string> cardName)
            {
                _provider = provider;
                _token = token;
                _cardName = cardName;
            }

            public override Card Target
            {
                get => _target ?? (_target = _provider.Get(_cardName()));
                set => _target = value;
            }
        }
    }
}
