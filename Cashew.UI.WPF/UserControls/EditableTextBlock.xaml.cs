using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cashew.UI.WPF.UserControls
{
    /// <summary>
    /// Interaction logic for EditableTextBlock.xaml
    /// </summary>
    public partial class EditableTextBlock : UserControl
    {
        readonly Guid _guid = Guid.NewGuid();

        public EditableTextBlock()
        {
            InitializeComponent();
            TextBlock.Text = (DataContext as string) ?? string.Empty;
            TextBox.Text = TextBlock.Text ?? string.Empty;
            DataContextChanged += OnDataContextChanged;
            SetToolTip();
        }

        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextBlock.Text = (DataContext as string) ?? string.Empty;
            TextBox.Text = TextBlock.Text ?? string.Empty;
        }
        
        void TextBlock_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!Interaction.IsDoubleClick(_guid))
                return;
            Initiate();
        }

        void TextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            // todo: key bindings
            if (e.Key == Key.Enter)
            {
                Confirm();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                Cancel();
                e.Handled = true;
            }
        }

        void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        void Initiate()
        {
            TextBox.Text = TextBlock.Text;
            TextBlock.Visibility = Visibility.Hidden;
            TextBox.Visibility = Visibility.Visible;
        }

        void Confirm()
        {
            TextBlock.Text = TextBox.Text;
            DataContext = TextBox.Text;
            TextBlock.Visibility = Visibility.Visible;
            TextBox.Visibility = Visibility.Hidden;
        }

        void Cancel()
        {
            TextBox.Text = TextBlock.Text;
            TextBlock.Visibility = Visibility.Visible;
            TextBox.Visibility = Visibility.Hidden;
        }

        public static readonly DependencyProperty FieldNameProperty = DependencyProperty.Register(
            "FieldName", typeof(string), typeof(EditableTextBlock), new PropertyMetadata(default(string)));

        public string FieldName
        {
            get { return (string) GetValue(FieldNameProperty); }
            set { SetValue(FieldNameProperty, value); }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == FieldNameProperty)
            {
                SetToolTip();
            }

            base.OnPropertyChanged(e);
        }

        void SetToolTip()
        {
            if (string.IsNullOrEmpty(FieldName))
            {
                ToolTip = "Double click to edit";
                return;
            }

            var stackPanel  = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.HorizontalAlignment = HorizontalAlignment.Left;
            var fieldName = new TextBlock();
            fieldName.Text = FieldName;
            fieldName.FontWeight = FontWeights.Bold;
            var message = new TextBlock();
            message.Text = "Double click to edit";
            stackPanel.Children.Add(fieldName);
            stackPanel.Children.Add(message);
            ToolTip = stackPanel;
        }
    }
}
