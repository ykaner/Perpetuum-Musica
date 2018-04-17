using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerpetuumMusica.View;

namespace PerpetuumMusica.ViewModel
{
    public class ConfirmDialogViewModel
    {
        public string Message { get; set; }

        private bool IsYes = false;

        private ConfirmDialog dialog;

        public bool ShowDialog(string message = "Are you sure?")
        {
            Message = message;
            dialog = new ConfirmDialog(this);
            dialog.ShowDialog();
            return IsYes;
        }

        private Command _CancelCommand;
        public Command CancellCommand => _CancelCommand ?? (_CancelCommand = new Command(Cancel));
        private Command _YesCommand;
        public Command YesCommand => _YesCommand ?? (_YesCommand = new Command(Yes));

        public void Cancel(object p)
        {
            IsYes = false;
            dialog.Close();
        }
        public void Yes(object p)
        {
            IsYes = true;
            dialog.Close();
        }
    }
}
