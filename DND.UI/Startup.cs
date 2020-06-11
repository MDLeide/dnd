using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DND.Data;
using DND.Data.DataAccess;
using DND.Domain;

namespace DND.UI
{
    static class Startup
    {
        public static void OnStartup(StartupEventArgs e)
        {
            Database.SetPath(GetDatabasePath());
            ConfigureDataAccess();
            InstallDatabase();
        }

        static void InstallDatabase()
        {
            if (Database.NeedsInstall())
            {
                Database.Install();
                InsertSystemRecords();
            }
        }

        static void InsertSystemRecords()
        {
            DataAccessManager.ImageDataAccess.Save(GetDefaultImage());
        }

        public static void ConfigureDataAccess()
        {
            var cardDataAccess = new CardDataAccess(
                null,
                null,
                null,
                null);

            DataAccessManager.CardDataAccess = cardDataAccess;
            DataAccessManager.ImageDataAccess = new ImageDataAccess();
            DataAccessManager.PropertyTypeDataAccess = new PropertyTypeDataAccess();
            DataAccessManager.CardTypeDataAccess = 
                new CardTypeDataAccess(DataAccessManager.ImageDataAccess, DataAccessManager.PropertyTypeDataAccess);
            DataAccessManager.PropertyDataAccess = 
                new PropertyDataAccess(DataAccessManager.CardDataAccess, DataAccessManager.PropertyTypeDataAccess);
            DataAccessManager.HardLinkDataAccess = 
                new HardLinkDataAccess(DataAccessManager.CardDataAccess);

            cardDataAccess.CardTypeDataAccess = DataAccessManager.CardTypeDataAccess;
            cardDataAccess.HardLinkDataAccess = DataAccessManager.HardLinkDataAccess;
            cardDataAccess.PropertyDataAccess = DataAccessManager.PropertyDataAccess;
            cardDataAccess.ImageDataAccess = DataAccessManager.ImageDataAccess;
        }

        static string GetDatabasePath()
        {
            return Path.Combine(
                Environment.CurrentDirectory,
                "dnddb.sqlite");
        }

        static Image GetDefaultImage()
        {
            var image = new Image();
            image.Location = "Images/ampersand.png";
            image.Name = "Default Image";
            image.Description = "Default image used when no other image is present.";
            return image;
        }
    }
}
