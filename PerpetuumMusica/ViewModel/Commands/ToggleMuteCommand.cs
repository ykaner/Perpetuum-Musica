using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PerpetuumMusica.ViewModel.Commands
{
    public class ToggleMuteCommand : ICommand
    {
        public ViewModel ViewModel { get; set; }

        public ToggleMuteCommand(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.ToggleMute(); 
        }
    }
}
