using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ListBox_Test
{
    class ShowCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //if (parameter != null) MessageBox.Show("not null");
            //else
            //    MessageBox.Show("null");

            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter == null) return;
            MessageBox.Show(parameter.ToString());
        }
    }
}
