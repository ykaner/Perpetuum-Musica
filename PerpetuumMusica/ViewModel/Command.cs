using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PerpetuumMusica.ViewModel
{
    class Command : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;
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
            else return canExecute(parameter);
            //the selected item of the ListBox is passed as parameter
            return parameter != null;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void RaiseCanExecuteChanged()
        {
            if ( CommandManager.RequerySuggested != null )

        }

        public void Execute(object parameter)
        {
            //the selected item of the ListBox is passed as parameter
            execute(parameter);
        }
    }
}
