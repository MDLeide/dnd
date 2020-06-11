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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DND.UI.Screens
{
    /// <summary>
    /// Interaction logic for AuthorView.xaml
    /// </summary>
    public partial class AuthorView : UserControl
    {
        const double SearchOpenLeft = 0;
        const double SearchClosedLeft = -200;
        const int SearchDurationMilliseconds = 500;

        readonly Duration _searchDuration = new Duration(new TimeSpan(0, 0, 0, 0, SearchDurationMilliseconds));

        bool _searchIsOpen;


        public AuthorView()
        {
            InitializeComponent();
        }

        void ShowSearch_OnClick(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation;

            if (_searchIsOpen)
                animation = new DoubleAnimation(SearchClosedLeft, _searchDuration);
            else
                animation = new DoubleAnimation(SearchOpenLeft, _searchDuration);

            _searchIsOpen = !_searchIsOpen;

            SearchGrid.BeginAnimation(Canvas.LeftProperty, animation);
        }
    }
}
