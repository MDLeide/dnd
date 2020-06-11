using Cashew.UI;

namespace DND.UI.Design
{
    class DesignCardTypeViewModel
    {
        public string Name { get; set; } = LorumIpsum.Generator.ConstructRandomSentence(
            2,
            5,
            true,
            true,
            null,
            null,
            string.Empty);

        public string Description { get; set; } = LorumIpsum.RandomAverageParagraph;
        public DesignImageViewModel Image { get; set; } = new DesignImageViewModel();
    }
}