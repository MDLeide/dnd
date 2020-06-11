using DND.Parser;

namespace DND
{
    public interface ICardNameResolver
    {
        string GetCardName(Token token);
    }
}