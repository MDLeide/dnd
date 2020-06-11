using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND.UI.Design
{
    class DesignCardSpaceManagementViewModel
    {
        public ObservableCollection<DesignCardSpaceViewModel> CardSpaces { get; set; }
        public DesignCardSpaceViewModel SelectedCardSpace { get; set; }
    }
}
