using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerpetuumMusica.View;
using PerpetuumMusica.Model;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace PerpetuumMusica.ViewModel
{
    public class AddTrackDialogViewModel : INotifyPropertyChanged
    {


        public string Address { get; set; }
        public string Title { get; set; }
        public string Composer { get; set; }
        public string ImagePath { get; set; }
        private bool defaultImage = true;

        private bool IsOk = false;

        private AddTrackDialog dialog;

        public Track ShowDialog(Playlist _ParentPlaylist = null)
        {
            ParentPlaylist = _ParentPlaylist;

            ResetFields();
            dialog = new AddTrackDialog(this);

            dialog.ShowDialog();
            if (IsOk)
            {
                ImageSource image = null;
                if (!defaultImage)
                    image = new BitmapImage(new Uri(ImagePath));

                return new Track(Title, Address, image, Composer); //Here the C'tor should find out the other properties
            }
                
            else
                return null;
        }

        private void ResetFields()
        {
            Address = ""; OnPropertyChanged("Address");
            Title = ""; OnPropertyChanged("Title");
            Composer = ""; OnPropertyChanged("Composer");
            ImagePath = GetResource.TrackImage(); OnPropertyChanged("ImagePath");
            defaultImage = true; //we reseted to the default image. 
            IsOk = false;
        }

        private Command _CancelCommand;
        public Command CancellCommand => _CancelCommand ?? (_CancelCommand = new Command(Cancel));
        private Command _OKCommand;
        private Command _ChangeImageCommand;
        public Command ChangeImageCommand => _ChangeImageCommand ?? (_ChangeImageCommand = new Command(ChangeImage));
        private Command _ChooseFileCommand;
        public Command ChooseFileCommand => _ChooseFileCommand ?? (_ChooseFileCommand = new Command(ChooseFile));

        private void ChangeImage(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ImagePath = openFileDialog.FileName;
                OnPropertyChanged("ImagePath");
                defaultImage = false; // we changed it so we get the flag off.
            }
            else
            {
                return;
            }
        }
        private void ChooseFile(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Address = openFileDialog.FileName;
                OnPropertyChanged("Address");
            }
            else
                return;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Command OKCommand => _OKCommand ?? (_OKCommand = new Command(OK));

        public void Cancel(object p)
        {
            IsOk = false;
            dialog.Close();
        }
        public void OK(object p)
        {
            IsOk = true;
            dialog.Close();
            
        }
    }
}
