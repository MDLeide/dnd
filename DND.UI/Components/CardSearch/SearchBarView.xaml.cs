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
    /// Interaction logic for SearchBarView.xaml
    /// </summary>
    public partial class SearchBarView : UserControl
    {
        bool _suppress;


        public SearchBarView()
        {
            DataContextChanged += OnDataContextChanged;
            HookupDataContext();
            InitializeComponent();
            Cards.SelectionChanged += CardsOnSelectionChanged;
        }


        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            HookupDataContext();
        }


        void SearchBar_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!(DataContext is CardSearchViewModel vm))
                return;

            if (e.Key == Key.Escape)
            {
                HideList();
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                //ApplicationCommands.Open.Execute(Cards.SelectedItem, this);

                if (vm.Cards.Count == 1)
                {
                    vm.SelectedCard = vm.Cards.FirstOrDefault();
                    vm.Open();
                    HideList();
                }
                else if (vm.SelectedCard != null)
                {
                    vm.Open();
                    HideList();
                }
                e.Handled = true;
            }
        }

        void SearchBar_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!(DataContext is CardSearchViewModel vm))
                return;

            if (e.Key == Key.Down)
            {
                if (Cards.SelectedIndex < Cards.Items.Count - 1)
                    IncrementSelectedIndex();
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (Cards.SelectedIndex > 0)
                    DecrementSelectedIndex();
                e.Handled = true;
            }
        }

        void SearchBar_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is CardSearchViewModel vm))
                return;

            if (vm.Cards.Any())
                ShowList();
        }

        void Cards_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!(DataContext is CardSearchViewModel vm))
                return;

            switch (e.Key)
            {
                case Key.Escape:
                    HideList();
                    SearchBar.Focus();
                    break;
                case Key.Enter:
                    vm.Open();
                    HideList();
                    break;
            }
        }

        void CardsOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_suppress || !(DataContext is CardSearchViewModel vm) || vm.SelectedCard == null)
                return;

            vm.Open();
            HideList();
        }

        void IncrementSelectedIndex()
        {
            _suppress = true;
            ++Cards.SelectedIndex;
            _suppress = false;
        }

        void DecrementSelectedIndex()
        {
            _suppress = true;
            --Cards.SelectedIndex;
            _suppress = false;
        }

        void HideList()
        {
            Cards.Visibility = Visibility.Hidden;
        }

        void ShowList()
        {
            Cards.Visibility = Visibility.Visible;
        }

        void HookupDataContext()
        {
            if (!(DataContext is CardSearchViewModel vm))
                return;

            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(CardSearchViewModel.Cards))
                    Cards.Visibility = vm.Cards.Any() ? Visibility.Visible : Visibility.Hidden;
            };

            if (vm.Cards != null)
                vm.Cards.CollectionChanged += (s, e) =>
                    Cards.Visibility = vm.Cards.Any() ? Visibility.Visible : Visibility.Hidden;
        }

    }
}
