using DND.Domain;
using DND.Parser;

namespace DND
{
    public interface ICardLinker
    {
        SoftLink Link(Token token);
        bool CanLink(Token token);
    }
}