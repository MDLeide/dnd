using Cashew.UI;
using DND.UI.Components;

namespace DND.UI.Design
{
    class DesignCardViewModel
    {
        public DesignCardViewModel()
        {
            LinkedText = Util.LinkedText.GetLinkedTextViewModel();
            LinkedText.Text = Description;
        }

        public string Title { get; set; } = LorumIpsum.Generator.ConstructRandomSentence(
            2,
            5,
            false,
            true,
            null,
            null,
            string.Empty);

        public string Subtitle { get; set; } = LorumIpsum.RandomShortSentence;
        public string Description { get; set; } = LorumIpsum.RandomPassage;

        public DesignImageViewModel PrimaryImage { get; set; } = new DesignImageViewModel();
        public DesignCardTypeViewModel CardType { get; set; } = new DesignCardTypeViewModel();
        public LinkedTextViewModel LinkedText { get; set; }
    }
}
