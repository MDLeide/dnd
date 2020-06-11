using System.Text;
using System.Threading.Tasks;

namespace Cashew.Data
{
    public interface IIdentifiable<T>
    {
        T Id { get; set; }
    }

    public interface IIdentifiable : IIdentifiable<int?> { }
}
