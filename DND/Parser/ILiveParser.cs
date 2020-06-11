using System.Collections.ObjectModel;

namespace DND.Parser
{
    public interface ILiveParser
    {
        string Text { get; }
        TokenCollectionChanges SetText(string val);
        ReadOnlyCollection<Token> Tokens { get; }
        Token FirstToken { get; }
        Token LastToken { get; }
        bool IsOpen { get; }
    }
}