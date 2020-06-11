using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DND.Domain;
using DND.UI.Domain;

namespace DND.UI.Util
{
    static class Images
    {
        const int DefaultImageId = 1;


        static Image _defaultImage;
        public static Image DefaultImage => 
            _defaultImage ?? (_defaultImage = DataAccessManager.ImageDataAccess?.Get(DefaultImageId) ?? new Image(){Name = "Default Image"});

        static ImageViewModel _imageViewModel;
        public static ImageViewModel DefaultImageViewModel => 
            _imageViewModel ?? (_imageViewModel = GetDefaultImageViewModel());

        public static ImageSource GetDefaultImageSource() => GetSource();

        public static Image DefaultCardTypeImage => DefaultImage;


        public static string CopyToLocalStore(string sourcePath)
        {
            var path = GetPath(sourcePath);
            File.Copy(sourcePath, path);
            return path;
        }

        static string GetPath(string sourcePath)
        {
            var ext = Path.GetExtension(sourcePath);
            var id = Guid.NewGuid().ToString();
            if (!ext.StartsWith("."))
                ext = "." + ext;
            return Path.Combine(Settings.ImageDirectory, $"{id}{ext}");
        }

        static Image GetDefaultImage()
        {
            var img = DataAccessManager.ImageDataAccess?.Get(DefaultImageId);

            if (img == null)
            {
                img = new Image();
                img.Name = "Default Image";
                img.Description = "Default image used when none other available.";
                img.Location = "";
            }

            return img;
        }

        static ImageViewModel GetDefaultImageViewModel()
        {
            return new ImageViewModel(
                GetDefaultImage(),
                GetSource());
        }

        static ImageSource GetSource()
        {
            //var bmp = new BitmapImage();
            //bmp.UriSource = new Uri("/Resources/Images/default.png", UriKind.Relative);
            //bmp.Freeze();
            //return bmp;
            var bmp = new BitmapImage(new Uri("/DND.UI;component/Resources/Images/default.png", UriKind.RelativeOrAbsolute));
            bmp.Freeze();
            return bmp;
            //return new BitmapImage(new Uri("/Resources/Images/default.png", UriKind.Absolute));
            //return new BitmapImage(new Uri("pack://application:,,,/Resources/Images/default.png", UriKind.Absolute));
        }
    }
}
