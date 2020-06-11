using System.Windows;
using System.Windows.Controls;

namespace DND.UI.Controls.Settings
{
    class SettingView : ContentControl
    {
        public SettingView()
        {
            
        }

        public static readonly DependencyProperty SettingNameProperty = DependencyProperty.Register(
            "SettingName", typeof(string), typeof(SettingView), new PropertyMetadata(default(string)));

        public string SettingName
        {
            get { return (string) GetValue(SettingNameProperty); }
            set { SetValue(SettingNameProperty, value); }
        }
    }
}
