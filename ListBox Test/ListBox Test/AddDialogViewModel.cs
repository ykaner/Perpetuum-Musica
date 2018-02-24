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

        private ICommand _approveCommand;
        public ICommand ApproveCommand => _approveCommand ?? (_approveCommand = new Command(Approve));

        public AddDialogViewModel()
        {
            
        }

        public void ShowDialog()
        {
            AddDialog dialog = new AddDialog(this);
            dialog.ShowDialog();
        }

        public void Approve(object p)
        {

        }


    }
}
