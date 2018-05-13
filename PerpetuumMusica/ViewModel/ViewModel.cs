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
using System.IO;
using System.Windows.Controls;

namespace PerpetuumMusica.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Model.Model model { get; set; }
        private AddTrackDialogViewModel AddTrackDialog { get; set; }
        private AddEmptyPlaylistDialogViewModel AddEmptyPlaylistDialog { get; set; }
        private ConfirmDialogViewModel ConfirmDialog { get; set; }
        //private View.Information InfoWindow;

        public ViewModel()
        {
            //initialize private fields: (Model, and Dialogs)
            model = new Model.Model();
            AddTrackDialog = new AddTrackDialogViewModel(model);
            AddEmptyPlaylistDialog = new AddEmptyPlaylistDialogViewModel(model);
            ConfirmDialog = new ConfirmDialogViewModel();

            model.MediaEnded += MediaEnded;
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
                new MenuItem("Track", Image("trackIcon"), new Command(AddTrack), null),
                new MenuItem("Playlist",Image("playlistIcon"), null, new List<MenuItem>()
                {
                    new MenuItem("Import", null, null, new List<MenuItem>()
                    {
                        new MenuItem("Youtube"),
                        new MenuItem("Local files")
                    }),
                    new MenuItem("Empty", Image("playlistIcon"), new Command(AddEmptyPlaylist))
                })
            };
            //Toolbar = new List<MenuItem>()
            //{
            //    new MenuItem("Info", Image("info_icon.png"), new Command(OpenInfoWindow), null),
            //    new MenuItem("Settings", Image("settings_icon.png"), null, null)
            //};

            //Init volume
            Volume = 80;

            //for testing:
            //ShowedItem = DemoData.Disney;
            //DemoData.SetParents(DemoData.musicals);

            //DB_connection.DB_conn mycon = new DB_connection.DB_conn();
            //ShowedItem = mycon.RetrievePlaylist(0)[0];
            



        }


        static public ImageSource Image(string name) { return GetResource.Image(name); }


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

        //private ImageSource disney = Image("disney");

        #region INotifyPropertyChanged Memberse

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        #region Commands

        /// <summary>
        /// Commands
        /// </summary>
        private Command _TogglePlayCommand;
        public Command TogglePlayCommand => _TogglePlayCommand ?? (_TogglePlayCommand = new Command(TogglePlay));
        private Command _TogglePlayItemCommand;
        public Command TogglePlayItemCommand => _TogglePlayItemCommand ?? (_TogglePlayItemCommand = new Command(TogglePlayItem));
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
        private Command _DeleteItemsCommand;
        public Command DeleteItemsCommand => _DeleteItemsCommand ?? (_DeleteItemsCommand = new Command(DeleteItems, DeleteItems_CanExecute));
        private Command _ShowInfoCommand;
        public Command ShowInfoCommand => _ShowInfoCommand ?? (_ShowInfoCommand = new Command(OpenInfoWindow));
        //private Command _AddTrackCommand;
        //public Command AddTrackCommand => _AddTrackCommand ?? (_AddTrackCommand = new Command(AddTrack));

        #endregion Commands

        #region properties to bind
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
                if (ShowedItem.IsPlaying)
                    return "Pause";
                else
                {
                    if (ShowedItem.IsOn)
                        return "Continue";
                    else
                        return "Play";
                }
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

        public PlaylistItem ShowedItem
        {
            get
            {
                return Model.ShowedItem;
            }
            set
            {
                Model.ShowedItem = value;
            }
        }
        public List<PlaylistItem> SelectedItems
        {
            get
            {
                try
                {
                    return ((Playlist)(ShowedItem.Content)).List.Where(item => item.IsSelected).ToList();
                }
                catch (Exception e)
                {
                    //if casting failed, this means we are in "Track" view, so there is no list. 
                    return new List<PlaylistItem>();
                }
                
            }
        }

        private Stack<PlaylistItem> ShowedItemHistory = new Stack<PlaylistItem>();
        private Stack<PlaylistItem> ShowedItemFuture = new Stack<PlaylistItem>(); 
        
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
        //public List<MenuItem> Toolbar { get; set; }

        //Events 
        void UpdateTrackSliderLocationTimer_tick(object sender, EventArgs e)
        {
            UpdateLocation();
        }
        #endregion
        
        #region methods
        //Playing Methods
        public void TogglePlay(object param = null)
        {
            Model.TogglePlay();
            OnPropertyChanged("ToggleButtonIcon");
            OnPropertyChanged("ToggleButtonText");

            UpdateLocation();
        }
        public void TogglePlayItem(object param)
        {
            PlaylistItem target = (PlaylistItem)param;
            model.TogglePlayItem(target);
            PlayingItemPropertyChanged();
            PlayingPropertyChanged();
            
        }
        private void MediaEnded(object sender, EventArgs e)
        {
            Model.TrackEnded();
            PlayingPropertyChanged();
            LocationPropertyChanged();
            PlayingItemPropertyChanged();
        }
        public void OpenItem(object param)
        {
            ShowedItemHistory.Push(ShowedItem);
            ShowedItemFuture.Clear();
            Model.Open((PlaylistItem)param);
            //--
            OnPropertyChanged("ShowedItem");
            OnPropertyChanged("ToggleButtonText");
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
        //Player methods
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
                LocationPropertyChanged();
            }
        }
        //nevigation methods
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
        //edit methods
        private List<PlaylistItem> objectToList(object param)
        {
            if (param == null) return null;

            //else:
            System.Collections.IList list = (System.Collections.IList)param;
            var collection = list.Cast<PlaylistItem>();
            return collection.ToList<PlaylistItem>();
        }
        public void DeleteItems(object param)
        {
            var targetItems = SelectedItems;

            string message = "Are you sure you want to delete the following items from the playlist \"" + ShowedItem + "\"? \n";

            foreach (PlaylistItem item in targetItems)
            {
                message += item + "\n";
            }

            if (ConfirmDialog.ShowDialog(message))
                Model.DeleteItems(targetItems, (Playlist)(ShowedItem.Content));
            OnPropertyChanged("ShowedItem");

            //if (param == null)
            //{
            //    MessageBox.Show("Parameter is null");
            //}
            //var items = SelectedItems;

            //string output = "Selected: \n";

            //if (items == null || items.Count == 0) output = "no item is selected";
            //else
            //{
            //    foreach (PlaylistItem item in items)
            //    {
            //        output += item.ToString() + "\n";
            //    }
            //}
            //MessageBox.Show(output);
        }
        public bool DeleteItems_CanExecute(object param)
        {
            return SelectedItems.Count > 0;
        }
        public void AddTrack(object param)
        {
            Track newTrack = AddTrackDialog.ShowDialog();
            if (newTrack == null) return;

            //MessageBox.Show("Adding new Track: \n " +
            //    "Title: " + newTrack.Title + "\n");

            Model.AddItem(newTrack, (Playlist)ShowedItem.Content);
        }
        public void AddEmptyPlaylist(object param)
        {
            Playlist newPlaylist = AddEmptyPlaylistDialog.ShowDialog();
            if (newPlaylist == null) return;

            Model.AddItem(newPlaylist, (Playlist)ShowedItem.Content);
        }

        public void OpenInfoWindow(object param)
        {
            View.Information InfoWindow = new View.Information();
            InfoWindow.ShowDialog();
        }
        #endregion

        #region property changed methods
        private void PlayingPropertyChanged()
        {
            OnPropertyChanged("IsPlaying");
            OnPropertyChanged("ToggleButtonIcon");
            OnPropertyChanged("ToggleButtonText");
        }
        private void LocationPropertyChanged()
        {
            OnPropertyChanged("TrackSliderLocation");
            OnPropertyChanged("TimeStamp");
        }
        private void PlayingItemPropertyChanged()
        {
            OnPropertyChanged("Playlist");
            OnPropertyChanged("CurrentlyPlayingIndex");
        }
        #endregion
    }
}
