using DND.Domain;

namespace DND.UI.Domain
{
    class HardLinkViewModel : DomainViewModel<HardLink>
    {
        public HardLinkViewModel(HardLink hardLink)
            : base(hardLink, DataAccessManager.HardLinkDataAccess) { }

        public HardLink HardLink => Object;
    }
}