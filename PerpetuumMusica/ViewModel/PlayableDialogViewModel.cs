using PerpetuumMusica.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace PerpetuumMusica.ViewModel
{
    abstract public class PlayableDialogViewModel : DialogViewModel
    {
        public string ImagePath { get; set; }

        public Playable Playable { get; set; } //will contain the new Track to be created

        protected bool defaultImage = true;


        private Command _ChangeImageCommand;
        public Command ChangeImageCommand => _ChangeImageCommand ?? (_ChangeImageCommand = new Command(ChangeImage));

        protected virtual void ResetFields()
        {
            //this.Playable = new Track();
            ImagePath = GetResource.TrackImage(); OnPropertyChanged("ImagePath");
            defaultImage = true; //we reseted to the default image. 
            IsOk = false;
        }

        protected void GetImageFromPath()
        {
            Playable.Image = null;
            if (!defaultImage) //if we are not using the default image, we need to get the new image from the path
                Playable.Image = new BitmapImage(new Uri(ImagePath));

        }

        protected void ChangeImage(object obj)
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

    }

}
