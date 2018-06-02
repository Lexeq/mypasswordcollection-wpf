using System;
using System.Windows.Input;

namespace MPC.ViewModels
{
    class Command : ICommand
    {
        private Func<bool> canExecuteAction;

        private Action executeAction;

        public Command(Action execute)
            : this(execute, null) { }

        public Command(Action execute, Func<bool> canExecute)
        {
            canExecuteAction = canExecute;
            executeAction = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (canExecuteAction != null)
            {
                return canExecuteAction();
            }
            return true;
        }

        public void Execute(object parameter)
        {
            executeAction?.Invoke();
        }
    }

    class Command<T> : ICommand
    {
        private Predicate<T> canExecuteAction;

        private Action<T> executeAction;

        public Command(Action<T> execute)
            : this(execute, null) { }

        public Command(Action<T> execute, Predicate<T> canExecute)
        {
            canExecuteAction = canExecute;
            executeAction = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (canExecuteAction != null)
            {
                return canExecuteAction((T)parameter);
            }
            return true;
        }

        public void Execute(object parameter)
        {
            executeAction?.Invoke((T)parameter);
        }
    }
}
