using System.Windows;
using System.Windows.Controls;

namespace DND.UI.Controls.Settings
{
    /// <summary>
    /// Interaction logic for MultiSelectionSettingView.xaml
    /// </summary>
    public partial class MultiSelectionSettingView : UserControl
    {
        public MultiSelectionSettingView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SettingNameProperty = DependencyProperty.Register(
            "SettingName", typeof(string), typeof(MultiSelectionSettingView), new PropertyMetadata(default(string)));

        public string SettingName
        {
            get { return (string) GetValue(SettingNameProperty); }
            set { SetValue(SettingNameProperty, value); }
        }
    }
}
