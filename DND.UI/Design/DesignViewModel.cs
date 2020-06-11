using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cashew.UI;

namespace DND.UI.Design
{
    public class DesignViewModel
    {
        public DesignViewModel()
        {
            ShortSentence = LorumIpsum.RandomShortSentence;
            AverageSentence = LorumIpsum.RandomAverageSentence;
            LongSentence = LorumIpsum.RandomLongSentence;

            ShortParagraph = LorumIpsum.RandomShortParagraph;
            AverageParagraph = LorumIpsum.RandomAverageParagraph;
            LongParagraph = LorumIpsum.RandomLongParagraph;

            Passage = LorumIpsum.RandomPassage;
        }

        public string ShortSentence { get; set; } 
        public string AverageSentence { get; set; }
        public string LongSentence { get; set; }

        public string ShortParagraph { get; set; }
        public string AverageParagraph { get; set; }
        public string LongParagraph { get; set; }

        public string Passage { get; set; }
    }
}
