using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PerpetuumMusica.ViewModel
{
    public abstract class DialogViewModel : INotifyPropertyChanged
    {
        protected Window dialog;

        protected bool IsOk = false;
        protected Model.Model Model { get; set; }

        protected Command _CancelCommand;
        public Command CancellCommand => _CancelCommand ?? (_CancelCommand = new Command(Cancel));
        protected Command _OKCommand;
        public Command OKCommand => _OKCommand ?? (_OKCommand = new Command(OK));

        protected abstract void CheckValidity();

        public void Cancel(object p)
        {
            IsOk = false;
            dialog.Close();
        }
        public void OK(object p)
        {
            //first we check if input is valid;
            try
            {
                CheckValidity();
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return; //if we encountered an exception, we do NOT close the window.
            }

            //if input is valid, we are intitled to procede. 
            IsOk = true;
            dialog.Close();

        }


        //---INotifyPropertyChanged fields: 
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
