using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using DND.UI.Domain;
using DND.UI.Screens.Select;
using Image = DND.Domain.Image;

namespace DND.UI.Controls.Settings
{
    /// <summary>
    /// Interaction logic for ImageSettingView.xaml
    /// </summary>
    public partial class ImageSettingView : UserControl
    {
        public ImageSettingView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SettingNameProperty = DependencyProperty.Register(
            "SettingName", typeof(string), typeof(ImageSettingView), new PropertyMetadata(default(string)));

        public string SettingName
        {
            get { return (string)GetValue(SettingNameProperty); }
            set { SetValue(SettingNameProperty, value); }
        }

        void Change_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = new SelectorViewModel<Image, ImageViewModel>(
                DataAccessManager.ImageDataAccess.Get().Select(p => new ImageViewModel(p)).ToArray,
                DataContext as ImageViewModel);
            var wm = new WindowManager();
            if (wm.ShowDialog(vm) ?? false)
                DataContext = vm.SelectedItem;
        }
    }
}
