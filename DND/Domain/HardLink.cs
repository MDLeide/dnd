namespace DND.Domain
{
    /// <summary>
    /// An explicit association between two cards.
    /// </summary>
    public class HardLink : DomainObject, ILink
    {
        public virtual bool Mutual { get; set; }
        public virtual Card Origin { get; set; }
        public virtual Card Target { get; set; }
    }
}