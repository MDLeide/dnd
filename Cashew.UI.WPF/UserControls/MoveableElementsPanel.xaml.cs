using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cashew.UI.WPF.UserControls
{
    /// <summary>
    /// Interaction logic for TableView.xaml
    /// </summary>
    public partial class MoveableElementsPanel : UserControl
    {
        readonly Dictionary<object, FrameworkElement> _objToElement = new Dictionary<object, FrameworkElement>();
        FrameworkElement _moving;
        Point _lastPosition;
        int _maxZ;


        public MoveableElementsPanel()
        {
            InitializeComponent();
        }
        

        public static readonly DependencyProperty InitialXOffsetProperty = DependencyProperty.Register(
            "InitialXOffset", typeof(double), typeof(MoveableElementsPanel), new PropertyMetadata(8d));

        public double InitialXOffset
        {
            get { return (double) GetValue(InitialXOffsetProperty); }
            set { SetValue(InitialXOffsetProperty, value); }
        }

        public static readonly DependencyProperty InitialYOffsetProperty = DependencyProperty.Register(
            "InitialYOffset", typeof(double), typeof(MoveableElementsPanel), new PropertyMetadata(15d));

        public double InitialYOffset
        {
            get { return (double) GetValue(InitialYOffsetProperty); }
            set { SetValue(InitialYOffsetProperty, value); }
        }

        public static readonly DependencyProperty YOffsetProperty = DependencyProperty.Register(
            "YOffset", typeof(double), typeof(MoveableElementsPanel), new PropertyMetadata(45d));

        public double YOffset
        {
            get { return (double) GetValue(YOffsetProperty); }
            set { SetValue(YOffsetProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(MoveableElementsPanel), new PropertyMetadata(default(IEnumerable), OnItemsSourcePropertyChanged));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem", typeof(object), typeof(MoveableElementsPanel), new PropertyMetadata(default(object)));

        public object SelectedItem
        {
            get { return (object) GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            "ItemTemplate", typeof(DataTemplate), typeof(MoveableElementsPanel), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate) GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (ItemsSource != null)
                BindItems(ItemsSource);
        }
        
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_moving == null)
                return;
            var pos = e.GetPosition(this);
            Canvas.SetTop(_moving, Canvas.GetTop(_moving) + pos.Y - _lastPosition.Y);
            Canvas.SetLeft(_moving, Canvas.GetLeft(_moving) + pos.X - _lastPosition.X);
            _lastPosition = pos;
        }


        static void OnItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is MoveableElementsPanel panel))
                return;
            
            panel.OnItemsSourceChanged(e.OldValue as IEnumerable, e.NewValue as IEnumerable);
        }


        void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (oldValue is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -= ItemsSourceCollectionChanged;
                foreach (var item in oldValue)
                    RemoveItem(item);
            }

            if (newValue is INotifyCollectionChanged newCollection)
            {
                newCollection.CollectionChanged += ItemsSourceCollectionChanged;
                BindItems(newValue);
            }
        }

        void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    BindItems(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                        RemoveItem(item);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        void BindItems(IEnumerable items)
        {
            var yOffset = GetNextY(InitialYOffset);

            foreach (var item in items)
            {
                BindItem(item, yOffset);
                yOffset = GetNextY(yOffset);
            }
        }

        void BindItem(object item, double yOffset)
        {
            if (!(ItemTemplate.LoadContent() is FrameworkElement ctrl))
                return;

            ctrl.DataContext = item;

            Canvas.Children.Add(ctrl);
            Canvas.SetTop(ctrl, yOffset);
            Canvas.SetLeft(ctrl, InitialXOffset);
            Panel.SetZIndex(ctrl, _maxZ++);

            ctrl.MouseLeftButtonDown += Element_OnMouseLeftButtonDown;
            ctrl.MouseLeftButtonUp += Element_OnMouseLeftButtonUp;
            ctrl.GotFocus += Element_OnGotFocus;
            ctrl.LostFocus += Element_OnLostFocus;

            _objToElement.Add(item, ctrl);
        }

        void RemoveItems(IEnumerable items)
        {
            foreach (var item in items)
                RemoveItem(item);
        }

        void RemoveItem(object item)
        {
            var fe = _objToElement[item];
            var zIndex = Panel.GetZIndex(fe);
            Canvas.Children.Remove(fe);
            foreach (var element in _objToElement.Values)
            {
                var index = Panel.GetZIndex(element);
                if (index > zIndex)
                    Panel.SetZIndex(element, index - 1);
            }

            _objToElement.Remove(item);
            _maxZ--;
        }

        void BringToFront(Canvas canvas, FrameworkElement element)
        {
            int currentIndex = Panel.GetZIndex(element);

            for (int i = 0; i < canvas.Children.Count; i++)
            {
                if (canvas.Children[i] is FrameworkElement fe && fe != element)
                {
                    int zIndex = Panel.GetZIndex(fe);
                    if (zIndex > currentIndex)
                        Panel.SetZIndex(fe, zIndex - 1);
                }
            }

            Panel.SetZIndex(element, _maxZ);
        }

        double GetNextY(double startingOffset)
        {
            var offset = startingOffset;
            while (ItemsSource.Cast<object>().Any(p =>
                _objToElement.ContainsKey(p) &&
                Math.Abs(Canvas.GetTop(_objToElement[p]) - offset) < .1))
                offset += YOffset;
            return offset;
        }


        void Element_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is FrameworkElement fe))
                return;

            if (SelectedItem == fe.DataContext)
                SelectedItem = null;
        }

        void Element_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is FrameworkElement fe))
                return;

            SelectedItem = fe.DataContext;
        }

        void Element_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _moving = null;
        }

        void Element_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var ctrl = sender as FrameworkElement;
            if (ctrl == null)
                return;

            _moving = ctrl;
            _lastPosition = e.GetPosition(this);
            BringToFront(Canvas, ctrl);
        }
    }
}
