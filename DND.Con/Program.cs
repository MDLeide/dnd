using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cashew.UI;

namespace DND.Con
{
    class Program
    {
        static void Main(string[] args)
        {
            TestLorumIpsum();
        }

        static void TestLorumIpsum()
        {
            TestIpsum("tiny sentence", LorumIpsum.RandomTinySentence);
            TestIpsum("short sentence", LorumIpsum.RandomShortSentence);
            TestIpsum("average sentence", LorumIpsum.RandomAverageSentence);
            TestIpsum("long sentence", LorumIpsum.RandomLongSentence);
            TestIpsum("huge sentence", LorumIpsum.RandomHugeSentence);
            TestIpsum("short paragraph", LorumIpsum.RandomShortParagraph);
            TestIpsum("average paragraph", LorumIpsum.RandomAverageParagraph);
            TestIpsum("long paragraph", LorumIpsum.RandomLongParagraph);
            TestIpsum("passage", LorumIpsum.RandomPassage);

            Console.WriteLine();
            Console.WriteLine("Any key to quit");
            Console.ReadKey(true);
        }

        static void TestIpsum(string type, string value)
        {
            Console.WriteLine($">>{type.ToUpper()} Press any key to see text:");
            Console.ReadKey(true);
            Console.WriteLine(value);
            Console.WriteLine();
        }

        static void TestStandardDeviation()
        {
            var values = new[]
            {
                727.7,
                1086.5,
                1091,
                1361.3,
                1490.5,
                1956.1
            };

            Console.WriteLine(Cashew.Math.Average(values));
            Console.WriteLine(Cashew.Math.StandardDeviation(values));
            Console.ReadKey();

        }

    }
}