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
using Cashew.UI.WPF;
using DND.UI.Components.IO;

namespace DND.UI.Domain.Card
{
    /// <summary>
    /// Interaction logic for AuthorView.xaml
    /// </summary>
    public partial class AuthorView : UserControl
    {
        readonly Guid _imageGuid = Guid.NewGuid();

        public AuthorView()
        {
            InitializeComponent();
        }

        void Image_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Interaction.IsDoubleClick(_imageGuid) && DataContext is CardViewModel vm)
                vm.ChangeImage();
        }

        void CardType_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is CardViewModel vm)
                vm.ChangeCardType();
        }
    }
}
