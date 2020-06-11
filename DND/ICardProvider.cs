using DND.Domain;
using DND.Parser;

namespace DND
{
    public interface ICardProvider
    {
        Card Get(string title);
    }
}