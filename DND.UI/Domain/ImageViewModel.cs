using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace DND.UI.Domain
{
    //todo: clean up orphaned image files
    public class ImageViewModel : DomainViewModel<DND.Domain.Image>
    {
        public ImageViewModel(DND.Domain.Image image)
            : this(image, GetImageSourceFromImage(image)) { }

        public ImageViewModel(DND.Domain.Image image, ImageSource source)
            : base(image, DataAccessManager.ImageDataAccess)
        {
            Source = source;
        }

        public DND.Domain.Image Image => Object;

        ImageSource _source;
        public ImageSource Source
        {
            get => _source ?? (_source = GetImageSourceFromImage(Image));
            set
            {
                if (Equals(value, _source)) return;
                _source = value;
                NotifyOfPropertyChange(nameof(Source));
            }
        }

        public string Name
        {
            get => Object.Name;
            set
            {
                Object.Name = value;
                NotifyOfPropertyChange(nameof(Name));
            }
        }

        public string Location
        {
            get => Object.Location;
            set
            {
                Object.Location = value;
                NotifyOfPropertyChange(nameof(Location));
                Source = new BitmapImage(new Uri(Location, UriKind.RelativeOrAbsolute));
            }
        }

        public string Description
        {
            get => Object.Description;
            set
            {
                Object.Description = value;
                NotifyOfPropertyChange(nameof(Description));
            }
        }

        public int Size
        {
            get => Object.Size;
            set
            {
                Object.Size = value;
                NotifyOfPropertyChange(nameof(Size));
            }
        }

        public void ChangeLocation()
        {
            var fb = new OpenFileDialog();
            fb.Filter = "Image Files|*.jpg;*.png;*.bmp";
            var result = fb.ShowDialog();
            if (!result ?? false)
                return;
            Location = Util.Images.CopyToLocalStore(fb.FileName);
            Source = GetImageSourceFromImage(Image);
        }

        public static ImageSource GetImageSourceFromImage(DND.Domain.Image image)
        {
            if (image == null || string.IsNullOrEmpty(image.Location))
                return null;

            var bmp = new BitmapImage(new Uri(image.Location, UriKind.Relative));
            bmp.Freeze();
            return bmp;
        }
    }
}