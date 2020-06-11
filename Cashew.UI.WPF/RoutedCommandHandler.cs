using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Cashew.UI.WPF
{
    /// <summary>
    ///  Allows associated a routed command with a non-routed command.  Used by
    ///  <see cref="RoutedCommandHandlers"/>.
    /// </summary>
    /// <remarks>
    /// https://stackoverflow.com/questions/36183366/how-can-i-handle-wpf-routed-commands-in-my-viewmodel-without-code-behind
    /// </remarks>
    public class RoutedCommandHandler : Freezable
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(RoutedCommandHandler),
            new PropertyMetadata(default(ICommand)));

        /// <summary> The command that should be executed when the RoutedCommand fires. </summary>
        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary> The command that triggers <see cref="ICommand"/>. </summary>
        public ICommand RoutedCommand { get; set; }

        /// <inheritdoc />
        protected override Freezable CreateInstanceCore()
        {
            return new RoutedCommandHandler();
        }

        /// <summary>
        ///  Register this handler to respond to the registered RoutedCommand for the
        ///  given element.
        /// </summary>
        /// <param name="owner"> The element for which we should register the command
        ///  binding for the current routed command. </param>
        internal void Register(FrameworkElement owner)
        {
            var binding = new CommandBinding(RoutedCommand, HandleExecuted, HandleCanExecute);
            owner.CommandBindings.Add(binding);
        }

        /// <summary> Proxy to the current Command.CanExecute(object). </summary>
        void HandleCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Command?.CanExecute(e.Parameter) == true;
            e.Handled = true;
        }

        /// <summary> Proxy to the current Command.Execute(object). </summary>
        void HandleExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Command?.Execute(e.Parameter);
            e.Handled = true;
        }
    }

    /// <summary>
    ///  Holds a collection of <see cref="RoutedCommandHandler"/> that should be
    ///  turned into CommandBindings.
    /// </summary>
    /// <remarks>
    /// https://stackoverflow.com/questions/36183366/how-can-i-handle-wpf-routed-commands-in-my-viewmodel-without-code-behind
    /// </remarks>
    public class RoutedCommandHandlers : FreezableCollection<RoutedCommandHandler>
    {
        /// <summary>
        ///  Hide this from WPF so that it's forced to go through
        ///  <see cref="GetCommands"/> and we can auto-create the collection
        ///  if it doesn't already exist.  This isn't strictly necessary but it makes
        ///  the XAML much nicer.
        /// </summary>
        static readonly DependencyProperty CommandsProperty = DependencyProperty.RegisterAttached(
          "CommandsPrivate",
          typeof(RoutedCommandHandlers),
          typeof(RoutedCommandHandlers),
          new PropertyMetadata(default(RoutedCommandHandlers)));

        /// <summary>
        ///  Gets the collection of RoutedCommandHandler for a given element, creating
        ///  it if it doesn't already exist.
        /// </summary>
        public static RoutedCommandHandlers GetCommands(FrameworkElement element)
        {
            RoutedCommandHandlers handlers = (RoutedCommandHandlers)element.GetValue(CommandsProperty);
            if (handlers == null)
            {
                handlers = new RoutedCommandHandlers(element);
                element.SetValue(CommandsProperty, handlers);
            }

            return handlers;
        }

        readonly FrameworkElement _owner;

        /// <summary> Each collection is tied to a specific element. </summary>
        /// <param name="owner"> The element for which this collection is created. </param>
        public RoutedCommandHandlers(FrameworkElement owner)
        {
            _owner = owner;

            // because we auto-create the collection, we don't know when items will be
            // added.  So, we observe ourself for changes manually. 
            var self = (INotifyCollectionChanged)this;
            self.CollectionChanged += (sender, args) =>
            {
                // note this does not handle deletions, that's left as an exercise for the
                // reader, but most of the time, that's not needed! 
                ((RoutedCommandHandlers)sender).HandleAdditions(args.NewItems);
            };
        }

        /// <summary> Invoked when new items are added to the collection. </summary>
        /// <param name="newItems"> The new items that were added. </param>
        void HandleAdditions(IList newItems)
        {
            if (newItems == null)
                return;

            foreach (RoutedCommandHandler routedHandler in newItems)
            {
                routedHandler.Register(_owner);
            }
        }

        /// <inheritdoc />
        protected override Freezable CreateInstanceCore()
        {
            return new RoutedCommandHandlers(_owner);
        }
    }
}