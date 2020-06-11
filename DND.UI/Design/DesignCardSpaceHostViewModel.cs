using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND.UI.Design
{
    class DesignCardSpaceHostViewModel
    {
        public DesignCardSearchViewModel CardSearch { get; set; } = new DesignCardSearchViewModel();
        public DesignCardSpaceViewModel CardSpace { get; set; } = new DesignCardSpaceViewModel();
    }
}
