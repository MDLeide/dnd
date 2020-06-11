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
using DND.Domain;
using DND.Parser;
using DND.UI.Util;

namespace DND.UI.Components.LinkedText
{
    /// <summary>
    /// Interaction logic for LinedDisplayView.xaml
    /// </summary>
    public partial class LinedDisplayView : UserControl
    {
        public LinedDisplayView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public static readonly DependencyProperty LineSpacingProperty = DependencyProperty.Register(
            "LineSpacing", typeof(double), typeof(LinedDisplayView), new PropertyMetadata(16d, LineSpacingChanged));

        public double LineSpacing
        {
            get { return (double) GetValue(LineSpacingProperty); }
            set { SetValue(LineSpacingProperty, value); }
        }

        public static readonly DependencyProperty InitialOffsetProperty = DependencyProperty.Register(
            "InitialOffset", typeof(double), typeof(LinedDisplayView), new PropertyMetadata(19d));

        public double InitialOffset
        {
            get { return (double) GetValue(InitialOffsetProperty); }
            set { SetValue(InitialOffsetProperty, value); }
        }

        public static readonly DependencyProperty LineBrushProperty = DependencyProperty.Register(
            "LineBrush", typeof(Brush), typeof(LinedDisplayView), new PropertyMetadata(new SolidColorBrush(Colors.DodgerBlue)));

        public Brush LineBrush
        {
            get { return (Brush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }

        public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register(
            "LineThickness", typeof(double), typeof(LinedDisplayView), new PropertyMetadata(1d));

        public double LineThickness
        {
            get { return (double)GetValue(LineThicknessProperty); }
            set { SetValue(LineThicknessProperty, value); }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            UpdateLines();
            base.OnRenderSizeChanged(sizeInfo);
        }

        static void LineSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LinedDisplayView v))
                return;

            v.DisplayText.LineHeight = (double) e.NewValue;
            v.UpdateLines();
        }

        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is LinkedTextViewModel ltvm))
                return;
            
            UpdateText(DisplayText, ltvm.Text, ltvm.Links);

            ltvm.PropertyChanged += (s, ev) =>
            {
                if (ev.PropertyName != nameof(LinkedTextViewModel.Text))
                    return;

                Application.Current.Dispatcher.Invoke(() =>
                    UpdateText(DisplayText, ltvm.Text, ltvm.Links));
            };

            ltvm.LinksUpdated += (s, ev) =>
                Application.Current.Dispatcher.Invoke(() =>
                    UpdateText(DisplayText, ltvm.Text, ltvm.Links));
        }

        void UpdateText(TextBlock display, string input, Dictionary<Token, SoftLink> links)
        {
            var position = 0;
            display.Inlines.Clear();
            foreach (var kvp in links.OrderBy(p => p.Key.Position))
            {
                // leading text
                if (kvp.Key.Position > position)
                {
                    var len = kvp.Key.Position - position;
                    display.Inlines.Add(InlineHelper.GetStringInline(input.Substring(position, len)));
                    position += len;
                }

                if (kvp.Key.IsOpen)
                    display.Inlines.Add(InlineHelper.GetTokenInline(kvp.Key));
                else if (kvp.Value == null)
                    display.Inlines.Add(InlineHelper.FormatBrokenLink(kvp.Key));
                else
                    display.Inlines.Add(InlineHelper.GetSoftLinkInline(kvp.Value));

                position += kvp.Key.RawValue.Length;
            }

            // trailing text
            if (position < input.Length - 1)
                display.Inlines.Add(InlineHelper.GetStringInline(input.Substring(position)));
            
            UpdateLines();
        }

        void UpdateLines()
        {
            Canvas.Children.Clear();
            var top = InitialOffset;
            
            while (top < ActualHeight)
            {
                var line = GetLine();
                Canvas.Children.Add(line);
                Canvas.SetTop(line, top);
                Canvas.SetLeft(line, 0);
                top += LineSpacing;
            }
        }

        Line GetLine()
        {
            var line = new Line();
            line.X1 = 0;
            line.X2 = ActualWidth;
            line.Stroke = LineBrush;
            line.StrokeThickness = LineThickness;
            return line;
        }
    }
}
