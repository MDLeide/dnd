namespace DND.Domain
{
    public class Category : DomainObject, INamed
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageLocation { get; set; }
    }
}