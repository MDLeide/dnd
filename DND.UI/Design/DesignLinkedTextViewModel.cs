using System.Threading;
using System.Threading.Tasks;
using Cashew.UI;
using DND.Parser;
using DND.UI.Components;
using DND.UI.Util;

namespace DND.UI.Design
{
    class DesignLinkedTextViewModel : LinkedTextViewModel
    {
        public DesignLinkedTextViewModel() 
            : base(LinkedText.CardLinker, new LiveParser(LinkedText.TokenParser))
        {
            Task.Run(() =>
            {
                Thread.Sleep(1500);
                Text = LorumIpsum.RandomPassage;
            });
        }
    }
}