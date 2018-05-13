using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Command
{
    public class Command : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;
        //private EventHandler _canExecuteChanged;

        public Command(Action<object> _execute, Func<object, bool> _canExecute)
        {
            execute = _execute;
            canExecute = _canExecute;
        }
        public Command(Action<object> _execute)
        {
            execute = _execute;
            canExecute = null;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
                return true;
            else
                return canExecute(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public void Execute(object parameter)
        {
            //the selected item of the ListBox is passed as parameter
            execute(parameter);
        }
    }
}
