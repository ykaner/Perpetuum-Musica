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
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace PerpetuumMusica.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Model.Model model;


        public ViewModel()
        {
            this.model = new Model.Model();
            //Set Commands
            this.TogglePlayCommand = new TogglePlayCommand(this);
            this.SearchCommand = new SearchCommand(this);
            this.ToggleMuteCommand = new ToggleMuteCommand(this);
            this.OpenAddMenuCommand = new OpenAddMenuCommand(this);

            //Set Timers
            UpdateTrackSliderLocationTimer = new DispatcherTimer();
            UpdateTrackSliderLocationTimer.Interval = TimeSpan.FromMilliseconds(5);
            UpdateTrackSliderLocationTimer.Tick += UpdateTrackSliderLocationTimer_tick;
            UpdateTrackSliderLocationTimer.Start();

            //Menues:
            MainMenu = new List<MenuItem>()
            {
                new MenuItem("Home ", null, null, null),
                new MenuItem("Currently Playing", null, null, null),
                new MenuItem("My Playlist", null, null, null)
            };

            //--
            AddMenu = new List<MenuItem>()
            {
                new MenuItem("Track", Image("track_icon.png"), null, null, "1"),
                new MenuItem("Playlist",Image("playlist_icon.png"), null, new List<MenuItem>()
                {
                    new MenuItem("Import"),
                    new MenuItem("Create")
                }, "2")

            };
            Toolbar = new List<MenuItem>()
            {
                new MenuItem("More Options", Image("menu_icon.png"), null, null),
                new MenuItem("Info", Image("info_icon.png"), null, null),
                new MenuItem("Settings", Image("settings_icon.png"), null, null)
            };

            //for tests:

        }

        //static private string sourceUri = @"C:\\Users\\Shachar\\source\\repos\\PerpetuumMusica\\PerpetuumMusica\\View\\Sources\\";
        //C:\\Users\\Shachar\\source\\repos\\PerpetuumMusica\\PerpetuumMusica\\View
        static private ImageSource Image(string name) { return (ImageSource) Application.Current.Resources[name]; }


        ////////////////////////
        /// private fields
        ////////////////////////
        //Images
        private ImageSource pauseIcon = Image("pauseIcon"); 
        private ImageSource playIcon = Image("playIcon");
        private ImageSource volumeIcon = Image("volumeIcon");
        private ImageSource volumeMuteIcon = Image("volumeMuteIcon");
        private ImageSource volumeLowIcon = Image("volumeLowIcon");
        private ImageSource trackIcon = Image("trackIcon");
        private ImageSource playlist = Image("playlistIcon");

        #region INotifyPropertyChanged Memberse

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        ////////////////////////
        /// public properties
        /// ////////////////////
        public Model.Model Model
        {
            get { return model; }
            set { }
        }

        //Timers
        DispatcherTimer UpdateTrackSliderLocationTimer;

        /// <summary>
        /// Commands
        /// </summary>
        public TogglePlayCommand TogglePlayCommand { get; set; }
        public SearchCommand SearchCommand { get; set; }
        public ToggleMuteCommand ToggleMuteCommand { get; set; }
        public OpenAddMenuCommand OpenAddMenuCommand { get; set; }
        /// <summary>
        /// Bindees
        /// </summary>

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
        public string ToggleButtonText
        {
            get
            {
                if (Model.IsPlaying)
                    return "Pause";
                else
                    return "Play";
            }
        }
        public ImageSource VolumeIcon
        {
            get
            {
                if (Volume > 50)
                    return volumeIcon;
                else if (Volume > 0)
                    return volumeLowIcon;
                else
                    return volumeMuteIcon;
            }
        }

        //this two are for changing the location manually
        private double nextLocation;
        private bool changeLocation = false;

        public double TrackSliderLocation {
            get
            {
                return Model.Player.LocationPercentage;
            }
            set
            {
                Model.Player.LocationPercentage = value;
                OnPropertyChanged("TimeStamp");
            }
        }
        public int Volume
        {
            get { return Model.Volume; }
            set
            {
                Model.Volume = value;
                OnPropertyChanged("VolumeIcon");
            }
        }
        public string TimeStamp
        {
            get { return Model.Player.GetProgressTime(); }
        }
        public bool AddMenuIsOpen { get; set; }

        //Menues
        public List<MenuItem> MainMenu { get; set; }
        public List<MenuItem> AddMenu { get; set; }
        public List<MenuItem> Toolbar { get; set; }

        //Events 
        void UpdateTrackSliderLocationTimer_tick(object sender, EventArgs e)
        {
            UpdateLocation();
        }

        //Methods
        public void TogglePlay()
        {
            Model.TogglePlay();
            OnPropertyChanged("ToggleButtonIcon");
            OnPropertyChanged("ToggleButtonText");

            UpdateLocation();
        }
        internal void ToggleMute()
        {
            Model.ToggleMute();
            OnPropertyChanged("Volume");
            OnPropertyChanged("VolumeIcon");
        }
        public void UpdateLocation()
        {
            if (Model.IsPlaying)
            {
                //if (changeLocation)
                //{
                //    Model.Player.LocationPercentage = nextLocation;
                //    changeLocation = false;
                //}

                //TrackSliderLocation = Model.Player.LocationPercentage;
                OnPropertyChanged("TrackSliderLocation");
                OnPropertyChanged("TimeStamp");
            }
        }
        public void Search(String query)
        {
            MessageBox.Show("Search Method: " + query);
        }
        public void OpenAddMenu()
        {
            AddMenuIsOpen = true;
            OnPropertyChanged("AddMenuIsOpen");
        }
    }
}
