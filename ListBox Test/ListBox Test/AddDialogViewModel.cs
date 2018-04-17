using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ListBox_Test
{
    public class AddDialogViewModel
    {
        public string Answer { get; set; }

        private AddDialog dialog;

        private ICommand _approveCommand;
        public ICommand ApproveCommand => _approveCommand ?? (_approveCommand = new Command(Approve));

        public AddDialogViewModel()
        {
            
        }

        public string ShowDialog()
        {
            dialog = new AddDialog(this);
            dialog.ShowDialog();
            return Answer;
        }

        public void Approve(object p)
        {
            dialog.Close();
        }


    }
}
