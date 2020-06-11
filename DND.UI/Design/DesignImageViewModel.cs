using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Cashew.UI;

namespace DND.UI.Design
{
    class DesignImageViewModel
    {
        public string Name { get; set; } = LorumIpsum.Generator.ConstructRandomSentence(
            2,
            6,
            true,
            true,
            null,
            null,
            string.Empty);

        public string Description { get; set; } = LorumIpsum.RandomShortParagraph;
        public string Location { get; set; } = "/DND.UI;component/Resources/Images/default.png";
        public ImageSource Source { get; set; } = Images.GetRandomImage();
    }
}