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
    /// Interaction logic for EditView.xaml
    /// </summary>
    public partial class EditView : UserControl
    {
        readonly Guid _imageGuid = Guid.NewGuid();

        public EditView()
        {
            InitializeComponent();
        }

        void Image_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(DataContext is CardViewModel vm))
                return;

            vm.ChangeImage();
        }

        void PropertyType_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(DataContext is CardViewModel vm))
                return;

            if (Interaction.IsDoubleClick(_imageGuid))
                vm.ChangeCardType();
        }
    }
}
