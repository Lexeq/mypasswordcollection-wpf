using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MPC.ViewModel
{
    class Command : ICommand
    {
        private Predicate<object> canExecuteAction;

        private Action<object> executeAction;

        public Command(Action<object> execute)
            : this(execute, null) { }

        public Command(Action<object> execute, Predicate<object> canExecute)
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
                return canExecuteAction(parameter);
            }
            return true;
        }

        public void Execute(object parameter)
        {
            executeAction?.Invoke(parameter);
        }
    }
}
