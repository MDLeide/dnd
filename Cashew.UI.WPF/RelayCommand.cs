using System;
using System.Windows.Input;

namespace Cashew.UI.WPF.MVVM
{
    public class RelayCommand : ICommand
    {
        readonly Func<object, bool> _canExecute;
        readonly Action<object> _execute;


        public RelayCommand(Action execute)
            : this(o => execute()) { }

        public RelayCommand(Action<object> execute)
            : this(execute, o => true) { }

        public RelayCommand(Action<object> execute, Func<bool> canExecute)
            : this(execute, o => canExecute()) { }

        public RelayCommand(Action execute, Func<bool> canExecute)
            : this (o => execute(), o => canExecute()) { }

        public RelayCommand(Action execute, Func<object, bool> canExecute)
            : this (o => execute(), canExecute) { }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }


        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void TriggerCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public event EventHandler CanExecuteChanged;
    }
}
