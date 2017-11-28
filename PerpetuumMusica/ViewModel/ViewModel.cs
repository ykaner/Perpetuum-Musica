using PerpetuumMusica.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PerpetuumMusica.Model;

namespace PerpetuumMusica.ViewModel
{
    public class ViewModel
    {
        private Model.Model model;


        public ViewModel()
        {
            this.model = new Model.Model();
            this.TogglePlayCommand = new TogglePlayCommand(this);
            
        }

        static private string sourceUri = "C:\\Users\\Shachar\\source\\repos\\PerpetuumMusica\\PerpetuumMusica\\View\\Sources\\";
        //private fields
        private ImageSource pauseIcon = new BitmapImage(new System.Uri(sourceUri + "pause_icon.png"));
        private ImageSource playIcon = new BitmapImage(new System.Uri(sourceUri + "play_icon.png"));

        //public properties
        public Model.Model Model
        {
            get { return model; }
            set { }
        }


        //Commands
        public TogglePlayCommand TogglePlayCommand { get; set; }

        //Images
        //TODO make a property of a "toggleButtonIcon" and bind it 
        public ImageSource ToggleButtonIcon
        {
            get
            {
                if (Model.IsPlaying)
                    return pauseIcon;
                else
                    return playIcon;
            }
        }
        
        //Methods
        public void TogglePlay()
        {
            Model.TogglePlay();
        }
    }
}
