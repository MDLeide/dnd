using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DND.Data;
using DND.Default;
using DND.Parser;
using DND.UI.Components;

namespace DND.UI.Util
{
    static class LinkedText
    {
        static ICardLinker _cardLinker;
        static ISoftLinkProvider _softLinkProvider;
        static ICardProvider _cardProvider;
        static ICardNameResolver _cardNameResolver;
        static ITokenParser _tokenParser;
        static IEnumerable<TokenType> _supportedTypes;
        static LinkTokenType _linkTokenType;

        public static LinkedTextViewModel GetLinkedTextViewModel()
        {
            return new LinkedTextViewModel(CardLinker, new LiveParser(TokenParser));
        }

        public static ISoftLinkProvider SoftLinkProvider =>
            _softLinkProvider ?? (_softLinkProvider = new SoftLinkProvider(TokenParser, CardLinker));

        public static IEnumerable<TokenType> SupportedTypes =>
            _supportedTypes ?? (_supportedTypes = new[] {new LinkTokenType()});

        public static TokenType LinkTokenType =>
            _linkTokenType ?? (_linkTokenType = new LinkTokenType());

        public static ITokenParser TokenParser =>
            _tokenParser ?? (_tokenParser = new TokenParser(SupportedTypes));

        public static ICardLinker CardLinker =>
            _cardLinker ?? (_cardLinker = new CardLinker(CardProvider, CardNameResolver));

        public static ICardProvider CardProvider =>
            _cardProvider ?? (_cardProvider = new CardProvider(DataAccessManager.CardDataAccess));

        public static ICardNameResolver CardNameResolver =>
            _cardNameResolver ?? (_cardNameResolver = new CardNameResolver());
    }
}