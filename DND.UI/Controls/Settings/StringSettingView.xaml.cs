using System.Windows;
using System.Windows.Controls;

namespace DND.UI.Controls.Settings
{
    /// <summary>
    /// Interaction logic for StringSettingView.xaml
    /// </summary>
    public partial class StringSettingView : UserControl
    {
        public StringSettingView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SettingNameProperty = DependencyProperty.Register(
            "SettingName", typeof(string), typeof(StringSettingView), new PropertyMetadata(default(string)));

        public string SettingName
        {
            get { return (string) GetValue(SettingNameProperty); }
            set { SetValue(SettingNameProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(object), typeof(StringSettingView), new PropertyMetadata(default(object)));

        public object Value
        {
            get { return (object) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
    }
}
