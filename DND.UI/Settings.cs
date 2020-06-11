using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND.UI
{
    [Serializable]
    public static class Settings
    {
        static SettingsObject _instance = new SettingsObject();

        public static void Load(SettingsObject settings) => _instance = settings;

        public static string ImageDirectory
        {
            get => _instance.ImageDirectory;
            set => _instance.ImageDirectory = value;
        }

        public static bool UtilitarianInterface
        {
            get => _instance.UtilitarianInterface;
            set => _instance.UtilitarianInterface = value;
        }

        public static class CardSettings
        {
            public static bool AutoSaveCardAfterChangingTitle
            {
                get => _instance.CardSettings.AutoSaveCardAfterChangingTitle;
                set => _instance.CardSettings.AutoSaveCardAfterChangingTitle = value;
            }

            public static bool AutoSaveCardAfterChangingDescription
            {
                get => _instance.CardSettings.AutoSaveCardAfterChangingDescription;
                set => _instance.CardSettings.AutoSaveCardAfterChangingDescription = value;
            }
        }

        public static class CardSpaceSettings
        {
            public static bool SetNewOpenedCardAsSelected
            {
                get => _instance.CardSpaceSettings.SetNewlyOpenedCardAsSelected;
                set => _instance.CardSpaceSettings.SetNewlyOpenedCardAsSelected = value;
            }
        }
    }

    public class SettingsObject
    {
        public string ImageDirectory { get; set; } = "images/";
        
        public bool UtilitarianInterface { get; set; }

        public CardSettingsObject CardSettings { get; set; } = new CardSettingsObject();
        public CardSpaceSettingsObject CardSpaceSettings { get; set; } = new CardSpaceSettingsObject();
    }

    public class CardSettingsObject
    {
        public bool AutoSaveCardAfterChangingTitle { get; set; }
        public bool AutoSaveCardAfterChangingDescription { get; set; }
    }

    public class CardSpaceSettingsObject
    {
        public bool SetNewlyOpenedCardAsSelected { get; set; } = true;
    }
}
