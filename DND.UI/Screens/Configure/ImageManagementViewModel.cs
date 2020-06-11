using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cashew.UI.WPF.MVVM;
using DND.DataAccess;
using DND.Domain;
using DND.UI.Domain;

namespace DND.UI.Screens.Configure
{
    class ImageManagementViewModel : Screen
    {
        public IImageDataAccess DataAccess { get; } = DataAccessManager.ImageDataAccess;

        ObservableCollection<ImageViewModel> _images;
        public ObservableCollection<ImageViewModel> Images
        {
            get => _images;
            set
            {
                if (Equals(value, _images)) return;
                _images = value;
                NotifyOfPropertyChange(nameof(Images));
            }
        }

        ImageViewModel _selectedImage;
        public ImageViewModel SelectedImage
        {
            get => _selectedImage;
            set
            {
                if (Equals(value, _selectedImage)) return;
                _selectedImage = value;
                NotifyOfPropertyChange(nameof(SelectedImage));
                NotifyOfPropertyChange(nameof(CanSave));
                NotifyOfPropertyChange(nameof(CanDelete));
            }
        }

        public void NewImage()
        {
            var image = new Image();
            image.Name = "New Image";
            image.Description = "Description";
            var vm = new ImageViewModel(image);
            Images.Add(vm);
            SelectedImage = vm;
        }

        public bool CanSave => SelectedImage != null;

        public void Save()
        {
            SelectedImage.Save();
        }

        public bool CanDelete => SelectedImage != null;

        public void Delete()
        {
            SelectedImage.Delete();
            Images.Remove(SelectedImage);
        }

        protected override object OnLoad(object context)
        {
            return DataAccess.Get().Select(p => new ImageViewModel(p)).ToArray();
        }

        protected override void PostLoad(object context, object loadContext)
        {
            Images = new ObservableCollection<ImageViewModel>((IEnumerable<ImageViewModel>) loadContext);
        }
    }
}
