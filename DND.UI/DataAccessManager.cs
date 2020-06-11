using DND.DataAccess;

namespace DND.UI
{
    static class DataAccessManager
    {
        public static ICardSpaceDataAccess CardSpaceDataAccess { get; set; }
        public static ICardSpaceCardDataAccess CardSpaceCardDataAccess { get; set; }
        public static IImageDataAccess ImageDataAccess { get; set; }
        public static ICardTypeDataAccess CardTypeDataAccess { get; set; }
        public static IHardLinkDataAccess HardLinkDataAccess { get; set; } 
        public static ICardDataAccess CardDataAccess { get; set; } 
        public static IPropertyDataAccess PropertyDataAccess { get; set; }
        public static IPropertyTypeDataAccess PropertyTypeDataAccess { get; set; }
    }
}