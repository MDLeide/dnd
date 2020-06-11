using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DND.UI.Design
{
    public static class Images
    {
        static readonly Random Rand = new Random();

        const int Characters = 0;
        const int Classes = 1;
        const int Factions = 2;
        const int Logos = 3;
        const int Monsters = 4;

        const string UriBase = "pack://application:,,,/DND.UI;component/Resources/Images/";

        static class ImageLocations
        {
            public static readonly string[] ImageTypes =
            {
                "Characters",
                "Classes",
                "Factions",
                "Logos",
                "Monsters"
            };

            public static readonly string[][] ImageNames =
            {
                new[] // characters
                {
                    "dwarf fighter.png",
                    "elf battlemage.png",
                    "elf ranger.png",
                    "human fighter.png"
                },
                new[] // classes
                {
                    "barbarian.png",
                    "bard.png",
                    "cleric.png",
                    "druid.png",
                    "fighter.png",
                    "monk.png",
                    "paladin.png",
                    "ranger.png",
                    "rogue.png",
                    "sorcerer.png",
                    "warlock.png",
                    "wizard.png"
                },
                new[] // factions
                {
                    "emerald enclave banner.png",
                    "emerald enclave circle.png",
                    "emerald enclave.png",
                    "harpers banner.png",
                    "harpers circle.png",
                    "harpers.png",
                    "lords alliance banner.png",
                    "lords alliance circle.png",
                    "lords alliance.png",
                    "order of the gauntlet banner.png",
                    "order of the gauntlet circle.png",
                    "order of the gauntlet.png",
                    "zhentarim banner.png",
                    "zhentarim circle.png",
                    "zhentarim.png"
                },
                new[] // logos
                {
                    "ampersand black.png",
                    "dnd black.png",
                    "dungeons & dragons black.png",
                    "stacked black.png",
                    "ampersand chrome.png",
                    "dnd chrome.png",
                    "dungeons & dragons chrome.png",
                    "stacked chrome.png",
                    "ampersand red.png",
                    "dnd red.png",
                    "dungeons & dragons red.png",
                    "stacked red.png",
                    "ampersand white.png",
                    "dnd white.png",
                    "dungeons & dragons white.png",
                    "stacked white.png"
                },
                new[] // monsters
                {
                    "flameskull.png",
                    "mimic.png",
                    "nothic.png",
                    "yuan-ti.png"
                }
            };
        }

        public static ImageSource GetRandomImage()
        {
            var typeIndex = Rand.Next(0, 5);
            return GetRandomImage(typeIndex);
        }

        public static ImageSource GetRandomCharacter()
        {
            return GetRandomImage(Characters);
        }

        public static ImageSource GetRandomClass()
        {
            return GetRandomImage(Classes);
        }

        public static ImageSource GetRandomFaction()
        {
            return GetRandomImage(Factions);
        }

        public static ImageSource GetRandomLogo()
        {
            return GetRandomImage(Logos);
        }

        public static ImageSource GetRandomMonster()
        {
            return GetRandomImage(Monsters);
        }

        

        static ImageSource GetRandomImage(int typeIndex)
        {
            var imageIndex = Rand.Next(0, ImageLocations.ImageNames[typeIndex].Length);
            return GetImage(typeIndex, imageIndex);
        }

        static ImageSource GetImage(int typeIndex, int imageIndex)
        {
            var uri = GetUri(typeIndex, imageIndex);
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
            bmp.EndInit();
            bmp.Freeze();
            return bmp;
        }

        static string GetUri(int typeIndex, int imageIndex)
        {
            var type = ImageLocations.ImageTypes[typeIndex];
            var image = ImageLocations.ImageNames[typeIndex][imageIndex];
            return GetUri(type, image);
        }

        static string GetUri(string imageType, string imageName)
        {
            return UriBase + imageType + "/" + imageName;
        }
    }
}