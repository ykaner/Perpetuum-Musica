using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PerpetuumMusica.ViewModel.Commands
{

    public class SearchCommand : ICommand
    {
        public ViewModel ViewModel { get; set; }

        public SearchCommand(ViewModel viewModel)
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
            String query = (String)parameter;
            ViewModel.Search(query);
        }
    }
}
