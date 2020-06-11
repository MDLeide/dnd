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
    /// Interaction logic for DisplayView.xaml
    /// </summary>
    public partial class DisplayView : UserControl
    {
        public DisplayView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
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
        }
    }
}
