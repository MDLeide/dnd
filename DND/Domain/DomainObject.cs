using Cashew.Data;

namespace DND.Domain
{
    // todo: change tracking
    public class DomainObject : IIdentifiable<int?>
    {
        public int? Id { get; set; }

        public override string ToString()
        {
            if (this is INamed named)
                return $"{GetType().Name}: {named.Name}";

            return base.ToString();
        }
    }
}