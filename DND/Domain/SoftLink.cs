namespace DND.Domain
{
    /// <summary>
    /// A reference to a card from within another card's text.
    /// </summary>
    public class SoftLink : ILink
    {
        public int Position { get; set; }
        public virtual Card Target { get; set; }
        public string Text { get; set; }
    }
}