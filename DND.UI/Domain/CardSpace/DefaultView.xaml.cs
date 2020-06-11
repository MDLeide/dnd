using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using DND.UI.Components;

namespace DND.UI.Domain.CardSpace
{
    /// <summary>
    /// Interaction logic for StandardView.xaml
    /// </summary>
    public partial class DefaultView : UserControl
    {
        readonly Dictionary<CardSpaceCardViewModel, FrameworkElement> _viewModelToElement = new Dictionary<CardSpaceCardViewModel, FrameworkElement>();

        // cheating hardcore here, fastest way to get it done
        CardSpaceViewModel _viewModel;
        FrameworkElement _moving;
        Point _lastPosition;
        int _largestZ;

        public DefaultView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }


        IEnumerable<CardSpaceCardViewModel> ViewModels => _viewModelToElement.Keys;

        public static readonly DependencyProperty InitialXOffsetProperty = DependencyProperty.Register(
            "InitialXOffset", typeof(double), typeof(DefaultView), new PropertyMetadata(8d));

        public double InitialXOffset
        {
            get { return (double)GetValue(InitialXOffsetProperty); }
            set { SetValue(InitialXOffsetProperty, value); }
        }

        public static readonly DependencyProperty InitialYOffsetProperty = DependencyProperty.Register(
            "InitialYOffset", typeof(double), typeof(DefaultView), new PropertyMetadata(15d));

        public double InitialYOffset
        {
            get { return (double)GetValue(InitialYOffsetProperty); }
            set { SetValue(InitialYOffsetProperty, value); }
        }

        public static readonly DependencyProperty YOffsetProperty = DependencyProperty.Register(
            "YOffset", typeof(double), typeof(DefaultView), new PropertyMetadata(45d));

        public double YOffset
        {
            get { return (double)GetValue(YOffsetProperty); }
            set { SetValue(YOffsetProperty, value); }
        }
        


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_viewModel != null)
                BindItems(_viewModel.Cards);
        }
        
        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is CardSpaceViewModel oldVm)
            {
                RemoveItems(oldVm.Cards);
            }
            if (e.NewValue is CardSpaceViewModel vm)
            {
                BindItems(vm.Cards);
                vm.Cards.CollectionChanged += CardsOnCollectionChanged;
                _viewModel = vm;
            }
        }

        void CardsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    BindItems(e.NewItems.Cast<CardSpaceCardViewModel>());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveItems(e.OldItems.Cast<CardSpaceCardViewModel>());
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RemoveItems(ViewModels);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void BindItems(IEnumerable<CardSpaceCardViewModel> items)
        {
            foreach (var item in items)
            {
                var element = GetElement();
                BindItem(item, element);
            }
        }

        void BindItem(CardSpaceCardViewModel item, FrameworkElement ctrl)
        {
            ctrl.DataContext = item;

            Caliburn.Micro.View.SetModel(ctrl, item);
            Caliburn.Micro.View.SetContext(ctrl, "IndexView");

            Canvas.Children.Add(ctrl);
            
            var topBinding = new Binding("PositionTop");
            topBinding.Source = item;
            ctrl.SetBinding(Canvas.TopProperty, topBinding);

            var leftBinding = new Binding("PositionLeft");
            leftBinding.Source = item;
            ctrl.SetBinding(Canvas.LeftProperty, leftBinding);

            var zBinding = new Binding("ZIndex");
            zBinding.Source = item;
            ctrl.SetBinding(Panel.ZIndexProperty, zBinding);

            if (item.ZIndex > _largestZ)
                _largestZ = item.ZIndex;
            else
            {
                IncrementZ(item.ZIndex);
                ++item.ZIndex;
            }

            ctrl.MouseLeftButtonDown += Element_OnMouseLeftButtonDown;
            ctrl.MouseLeftButtonUp += Element_OnMouseLeftButtonUp;
            ctrl.GotFocus += Element_OnGotFocus;
            ctrl.LostFocus += Element_OnLostFocus;

            _viewModelToElement.Add(item, ctrl);
        }

        FrameworkElement GetElement()
        {
            return new ContentControl();
        }

        void IncrementZ(int from)
        {
            foreach (var card in _viewModel.Cards)
                if (card.ZIndex > from)
                    card.ZIndex++;
        }
        
        void RemoveItems(IEnumerable<CardSpaceCardViewModel> items)
        {
            foreach (var item in items)
                RemoveItem(item);
        }

        void RemoveItem(CardSpaceCardViewModel item)
        {
            foreach (var vm in ViewModels)
                if (vm.ZIndex > item.ZIndex)
                    --vm.ZIndex;

            var fe = _viewModelToElement[item];
            Canvas.Children.Remove(fe);

            _viewModelToElement.Remove(item);
            _largestZ--;
        }

        void BringToFront(CardSpaceCardViewModel vm)
        {
            foreach (var item in ViewModels)
                if (item.ZIndex > vm.ZIndex)
                    --vm.ZIndex;

            vm.ZIndex = _largestZ;
        }

        void BringToFront(FrameworkElement fe)
        {
            var oldIndex = Panel.GetZIndex(fe);
            if (oldIndex >= _largestZ)
                return;

            foreach (var child in Canvas.Children.Cast<object>())
                if (child is UIElement ue)
                    if (Panel.GetZIndex(ue) > oldIndex)
                        Panel.SetZIndex(ue, Panel.GetZIndex(ue) - 1);

            Panel.SetZIndex(fe, _largestZ);
        }

        double GetNextY(double startingOffset)
        {
            while (ViewModels.Any(p => p.PositionTop.Equals(startingOffset)))
                startingOffset += YOffset;
            return startingOffset;
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_moving == null)
                return;

            var pos = e.GetPosition(this);

            if (_moving.DataContext is CardSpaceCardViewModel vm)
            {
                vm.PositionTop = vm.PositionTop + pos.Y - _lastPosition.Y;
                vm.PositionLeft = vm.PositionLeft + pos.X - _lastPosition.X;
            }
            else
            {
                Canvas.SetTop(_moving, Canvas.GetTop(_moving) + pos.Y - _lastPosition.Y);
                Canvas.SetLeft(_moving, Canvas.GetLeft(_moving) + pos.X - _lastPosition.X);
            }

            _lastPosition = pos;
        }

        void Element_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is FrameworkElement fe) || !(fe.DataContext is CardSpaceCardViewModel vm))
                return;

            if (_viewModel.SelectedCard == vm)
                _viewModel.SelectedCard = null;
        }

        void Element_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is FrameworkElement fe) || !(fe.DataContext is CardSpaceCardViewModel vm))
                return;

            _viewModel.SelectedCard = vm;
        }

        void Element_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _moving = null;
        }

        void Element_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement fe))
                return;

            _moving = fe;
            _lastPosition = e.GetPosition(this);

            if (!(fe.DataContext is CardSpaceCardViewModel vm))
            {
                BringToFront(fe);
                return;
            }

            BringToFront(vm);
        }
    }
}
