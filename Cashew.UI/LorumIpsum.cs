using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashew.UI
{
    public static class LorumIpsum
    {
        // todo: calculate counts as bell curve based on real data

        public static LorumIpsumGenerator Generator { get; set; } = new LorumIpsumGenerator();

        public static int MinTitleWordCount { get; set; } = 2;
        public static int MaxTitleWordCount { get; set; } = 6;

        public static int MinTinySentenceWordCount { get; set; } = 4;
        public static int MaxTinySentenceWordCount { get; set; } = 8;

        public static int MinShortSentenceWordCount { get; set; } = 8;
        public static int MaxShortSentenceWordCount { get; set; } = 14;

        public static int MinAverageSentenceWordCount { get; set; } = 14;
        public static int MaxAverageSentenceWordCount { get; set; } = 26;

        public static int MinLongSentenceWordCount { get; set; } = 26;
        public static int MaxLongSentenceWordCount { get; set; } = 30;

        public static int MinHugeSentenceWordCount { get; set; } = 30;
        public static int MaxHugeSentenceWordCount { get; set; } = 36;


        public static int MinShortParagraphSentenceCount { get; set; } = 2;
        public static int MaxShortParagraphSentenceCount { get; set; } = 4;

        public static int MinAverageParagraphSentenceCount { get; set; } = 4;
        public static int MaxAverageParagraphSentenceCount { get; set; } = 7;

        public static int MinLongParagraphSentenceCount { get; set; } = 7;
        public static int MaxLongParagraphSentenceCount { get; set; } = 12;


        public static int MinPassageParagraphCount { get; set; } = 3;
        public static int MaxPassageParagraphCount { get; set; } = 8;


        public static string RandomTitle =>
            Generator.ConstructRandomSentence(
                MinTitleWordCount,
                MaxTitleWordCount,
                true,
                true,
                null,
                null,
                string.Empty);

        public static string RandomTinySentence =>
            Generator.ConstructRandomSentence(MinTinySentenceWordCount, MaxTinySentenceWordCount);

        public static string RandomShortSentence =>
            Generator.ConstructRandomSentence(MinShortSentenceWordCount, MaxShortSentenceWordCount);

        public static string RandomAverageSentence =>
            Generator.ConstructRandomSentence(MinAverageSentenceWordCount, MaxAverageSentenceWordCount);

        public static string RandomLongSentence =>
            Generator.ConstructRandomSentence(MinLongSentenceWordCount, MaxLongSentenceWordCount);

        public static string RandomHugeSentence =>
            Generator.ConstructRandomSentence(MinHugeSentenceWordCount, MaxHugeSentenceWordCount);


        public static string RandomShortParagraph => Generator.ConstructRandomParagraphFromWords(
            MinShortParagraphSentenceCount,
            MaxShortParagraphSentenceCount);

        public static string RandomAverageParagraph => Generator.ConstructRandomParagraphFromWords(
            MinAverageParagraphSentenceCount,
            MaxAverageParagraphSentenceCount);

        public static string RandomLongParagraph => Generator.ConstructRandomParagraphFromWords(
            MinLongParagraphSentenceCount,
            MaxLongParagraphSentenceCount);


        public static string RandomPassage => Generator.ConstructRandomPassageFromWords(
            MinPassageParagraphCount,
            MaxPassageParagraphCount);
    }
}
