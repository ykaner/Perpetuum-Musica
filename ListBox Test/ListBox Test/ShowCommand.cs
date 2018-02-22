using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ListBox_Test
{
    class ShowCommand : ICommand
    {
        private event EventHandler _CanExecuteChanged;
        public event EventHandler CanExecuteChanged
        {
            add { _CanExecuteChanged += value; }
            remove { _CanExecuteChanged -= value; }
        }

        internal void RaiseCanExecuteChanged()
        {
            if (_CanExecuteChanged != null)
                _CanExecuteChanged(this, EventArgs.Empty);
        }

        ViewModel vm;

        public ShowCommand(ViewModel _vm)
        {
            vm = _vm;
        }

        public bool CanExecute(object parameter)
        {
            if (vm.SelectedIndex == -1) return false;
            else return true;

        }

        public void Execute(object parameter)
        {
            ExecuteSingle(parameter);
        }

        private void ExecuteSingle(object parameter)
        {
            MessageBox.Show(parameter.ToString());
        }

        private void ExecuteMulti(object parameter)
        {
            if (parameter == null)
            {
                MessageBox.Show("input is null");
                return;
            }

            var itemsList = (parameter as ObservableCollection<object>).Cast<string>().ToList();

            string output = "selected items: \n";
            foreach (string item in itemsList)
            {
                output += item.ToString() + "\n";
            }

            MessageBox.Show(output);
        }
    }
}
