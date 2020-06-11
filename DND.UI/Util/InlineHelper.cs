using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using DND.Domain;
using DND.Parser;
using DND.UI.Domain;

namespace DND.UI.Util
{
    static class InlineHelper
    {
        public static Inline GetBrokenLinkInline(Token token) =>
            FormatBrokenLink(token);

        public static Inline GetSoftLinkInline(SoftLink link) =>
            GetHyperlink(link);

        public static Inline GetTokenInline(Token token) =>
            token.IsOpen ? GetOpenTokenInline(token) : FormatToken(token);

        public static Inline GetOpenTokenInline(Token token)
            => FormatOpenToken(token);

        public static Inline GetStringInline(string val) =>
            FormatString(val);

        static Inline FormatRun(string val, Format format)
        {
            if (format == null)
                return new Run(val);

            return format.FormatRun(val);
        }


        public static Action<object, RoutedEventArgs, SoftLink> HandleLinkMouseClick { get; set; } = DefaultHandleMouseClick;
        public static Action<object, MouseEventArgs, SoftLink> HandleLinkMouseEnter { get; set; } = DefaultHandleMouseEnter;
        public static Func<Token, Inline> FormatOpenToken { get; set; } = DefaultFormatOpenToken;
        public static Func<Token, Inline> FormatToken { get; set; } = DefaultFormatToken;
        public static Func<Token, Inline> FormatBrokenLink { get; set; } = DefaultFormatBrokenLink;
        public static Func<string, Inline> FormatString { get; set; } = DefaultFormatString;
        public static Func<SoftLink, Inline> FormatSoftLink { get; set; } = DefaultFormatSoftLink;
        public static Func<SoftLink, Hyperlink> GetHyperlink { get; set; } = DefaultGetHyperlink;


        static void DefaultHandleMouseClick(object sender, RoutedEventArgs args, SoftLink link)
        {
            // todo: handle links
            //if (ScreenStack.ActiveCardSpace != null)
            //    ScreenStack.ActiveCardSpace.CardSpace.AddCard(link.Target);
        }

        static void DefaultHandleMouseEnter(object sender, MouseEventArgs args, SoftLink link)
        {

        }

        static Inline DefaultFormatBrokenLink(Token token) => FormatRun(token.Value, Formatting.BrokenLinkFormat);
        static Inline DefaultFormatOpenToken(Token token) => FormatRun(token.Value, Formatting.OpenTokenFormat);
        static Inline DefaultFormatToken(Token token) => FormatRun(token.Value, Formatting.TokenFormat);
        static Inline DefaultFormatString(string val) => FormatRun(val, Formatting.StringFormat);
        static Inline DefaultFormatSoftLink(SoftLink link) => FormatRun(link.Text, Formatting.SoftLinkFormat);
        static Hyperlink DefaultGetHyperlink(SoftLink link)
        {
            var hyperlink = new Hyperlink(FormatSoftLink(link));
            hyperlink.Click += (s, ev) => HandleLinkMouseClick(s, ev, link);
            hyperlink.MouseEnter += (s, ev) => HandleLinkMouseEnter(s, ev, link);
            return hyperlink;
        }
    }
}
