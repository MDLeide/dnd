using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DND.UI.Domain.CardSpaceCard
{
    /// <summary>
    /// Interaction logic for StandardView.xaml
    /// </summary>
    public partial class DefaultView : UserControl
    {
        public DefaultView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ContextProperty = DependencyProperty.Register(
            "Context",
            typeof(string),
            typeof(DefaultView),
            new PropertyMetadata(default(string)));

        public string Context
        {
            get { return (string) GetValue(ContextProperty); }
            set { SetValue(ContextProperty, value); }
        }
    }
}
