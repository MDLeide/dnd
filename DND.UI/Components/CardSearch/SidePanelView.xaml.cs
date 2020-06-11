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

namespace DND.UI.Components.CardSearch
{
    /// <summary>
    /// Interaction logic for SidePanelView.xaml
    /// </summary>
    public partial class SidePanelView : UserControl
    {
        public SidePanelView()
        {
            InitializeComponent();
        }

        void Cards_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is CardSearchViewModel vm && vm.SelectedCard != null)
                vm.Open();
        }
    }
}
