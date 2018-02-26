//using PerpetuumMusica.ViewModel.Commands;
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
using System.Windows.Input;

namespace PerpetuumMusica.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Model.Model model;


        public ViewModel()
        {
            this.model = new Model.Model();

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
                new MenuItem("Track", Image("playlistIcon"), null, null, "1"),
                new MenuItem("Playlist",Image("trackIcon"), null, new List<MenuItem>()
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

            //Init volume
            Volume = 80;

            //fortesting:
            ShowedItem = DemoData.DemoItem;
           

        }

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
        private Command _TogglePlayCommand;
        public Command TogglePlayCommand => _TogglePlayCommand ?? (_TogglePlayCommand = new Command(TogglePlay));
        private Command _PlayAtCommand;
        public Command PlayAtCommand => _PlayAtCommand ?? (_PlayAtCommand = new Command(PlayAt));
        private Command _SearchCommand;
        public Command SearchCommand => _SearchCommand ?? (_SearchCommand = new Command(Search));
        private Command _ToggleMuteCommand;
        public Command ToggleMuteCommand => _ToggleMuteCommand ?? (_ToggleMuteCommand = new Command(ToggleMute));
        private Command _OpenItemCommand;
        public Command OpenItemCommand => _OpenItemCommand ?? (_OpenItemCommand = new Command(OpenItem));
        private Command _HistoryBackCommand;
        public Command HistoryBackCommand => _HistoryBackCommand ?? (_HistoryBackCommand = new Command(HistoryBack, CanGoBack));
        private Command _HistoryForewardCommand;
        public Command HistoryForewardCommand => _HistoryForewardCommand ?? (_HistoryForewardCommand = new Command(HistoryForeward, CanGoForeward));
        private Command _OpenAddMenu;
        public Command OpenAddMenuCommand => _OpenAddMenu ?? (_OpenAddMenu = new Command(OpenAddMenu));


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

        public PlaylistItem ShowedItem { get; set; }
        private Stack<PlaylistItem> ShowedItemHistory = new Stack<PlaylistItem>();
        private Stack<PlaylistItem> ShowedItemFuture = new Stack<PlaylistItem>(); 
        
        //public ObservableCollection<PlaylistItem> ShowedPlaylist { get; set; }
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
        public double Volume
        {
            get { return Model.Volume * 100; }
            set
            {
                Model.Volume = value / 100;
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
        public void TogglePlay(object param = null)
        {
            Model.TogglePlay();
            OnPropertyChanged("ToggleButtonIcon");
            OnPropertyChanged("ToggleButtonText");

            UpdateLocation();
        }
        public void PlayAt(object param)
        {
            PlaylistItem target = (PlaylistItem)param;
            model.PlayAt(target); 
            OnPropertyChanged("Playlist");
            OnPropertyChanged("CurrentlyPlayingIndex");
        }
        public void OpenItem(object param)
        {
            ShowedItemHistory.Push(ShowedItem);
            ShowedItemFuture.Clear();
            ShowedItem = (PlaylistItem)param;
            OnPropertyChanged("ShowedItem");
            HistoryBackCommand.RaiseCanExecuteChanged(); 

        }
        //History Methods
        public bool CanGoBack(object param)
        {
            return (ShowedItemHistory.Count != 0);
        }
        public bool CanGoForeward(object param)
        {
            return (ShowedItemFuture.Count != 0);
        } 
        public void HistoryBack(object param)
        {
            if (!CanGoBack(param)) return;

            ShowedItemFuture.Push(ShowedItem);
            ShowedItem = ShowedItemHistory.Pop();
            OnPropertyChanged("ShowedItem");
            HistoryBackCommand.RaiseCanExecuteChanged();
            HistoryForewardCommand.RaiseCanExecuteChanged();
        }
        public void HistoryForeward(object param)
        {
            if (!CanGoForeward(param)) return;

            ShowedItemHistory.Push(ShowedItem);
            ShowedItem = ShowedItemFuture.Pop();
            OnPropertyChanged("ShowedItem");
            HistoryBackCommand.RaiseCanExecuteChanged();
            HistoryForewardCommand.RaiseCanExecuteChanged();
        }
        internal void ToggleMute(object param = null)
        {
            Model.ToggleMute();
            OnPropertyChanged("Volume");
            OnPropertyChanged("VolumeIcon");
        }
        public void UpdateLocation()
        {
            if (Model.IsPlaying)
            {
                OnPropertyChanged("TrackSliderLocation");
                OnPropertyChanged("TimeStamp");
            }
        }
        public void Search(object param)
        {
            string query = (string)param;

            MessageBox.Show("Search Method: " + query);
        }
        public void OpenAddMenu(object param)
        {
            AddMenuIsOpen = true;
            OnPropertyChanged("AddMenuIsOpen");
        }
    }
}
