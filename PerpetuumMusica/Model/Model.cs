using Player;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using DB_connection;

namespace PerpetuumMusica.Model
{
    public class Model
    {
        #region ctor
        public Model()
        {
            Volume = 70;
            IsPlaying = false;
            Player.MediaEnded += Player_MediaEnded;
            //set the showed Item
            //ShowedItem = DataBase.RetrievePlaylist(0)[0];

            Load();
        }
        #endregion

        #region Members
        private DB_conn DataBase = new DB_conn();
        public AudioPlayer Player = new AudioPlayer();

        //public int currentlyPlayingIndex = -1; //-1 note nothing is played;
        private PlaylistItem currentlyPlayingItem { get; set; }
        public PlaylistItem ShowedItem { get; internal set; }

        //playing information
        private bool isPlaying = true;
        private double location = 0;

        #endregion

        #region Data loading functions
        private void Load()
        {
            ShowedItem = new PlaylistItem(new Playlist("All Playlists", null, new TimeSpan(), 0, "Various Artists", null));
            ((Playlist)ShowedItem.Content).List = new ObservableCollection<PlaylistItem>( DataBase.RetrievePlaylist(0));

        }
        public void Open(PlaylistItem target)
        {
            ShowedItem = target;
            try //if it's a playlist, load the internal list
            {
                Playlist targetPlaylist = (Playlist)target.Content;

                //load internal list from dataBase (if not already loaded)
                if (!targetPlaylist.ListLoaded)
                {
                    targetPlaylist.List = new ObservableCollection<PlaylistItem>(DataBase.RetrievePlaylist(target.ID));
                    targetPlaylist.ListLoaded = true;
                }
            }
            catch
            {
                //
                return;
            }
            
                
        }
        #endregion

        #region volume functions
        private double unmutedVolume;
        internal void ToggleMute()
        {
            if (Volume > 0)
            {
                unmutedVolume = Volume;
                Volume = 0;
            }
            else
            {
                Volume = unmutedVolume;
            }
        }
        public double Volume
        {
            get { return Player.Volume; }
            set
            {
                Player.Volume = value;
            }
        }
        #endregion

        #region playing functions
        public double LocationPercentage
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }
        public bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
            set
            {
                isPlaying = value;
            }
        }
        public void TogglePlay()
        {
            if (IsPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }

            //IsPlaying = !IsPlaying;
        }
        public void Pause()
        {
            Player.Pause();
            currentlyPlayingItem.IsPlaying = false;
            IsPlaying = false;
        }
        public void Play()
        {
            if (currentlyPlayingItem == null)
            {
                MessageBox.Show("Play \"showed\" Item (not implemented)");
                return;
            }
            else
                currentlyPlayingItem.IsPlaying = true;

            Player.Play();

            IsPlaying = true;
        }
        public event EventHandler MediaEnded;
        private void Player_MediaEnded(object sender, EventArgs e)
        {
            if (MediaEnded != null)
            {
                MediaEnded(this, e);
            }
        }
        public void TrackEnded()
        {
            //currentlyPlayingItem = currentlyPlayingItem.Next;
            //Option 1 - play next in current list
            if (currentlyPlayingItem.Next != null)
            {
                PlayItem(currentlyPlayingItem.Next);
            }
            //no item to play - pause
            else
            {
                Pause();
                Player.SetTime(0);
            }
            
        }

        public void TogglePlayItem(PlaylistItem item)
        {
            //if we toggle to currently played item, we need to pause it
            if (item.IsOn == true)
            {
                item.IsPlaying = !item.IsPlaying;
                TogglePlay();
                return;
            }
            //we need to play the item
            else
            {
                PlayItem(item);
            }
            
        }
        private void PlayItem(PlaylistItem item)
        {
            //if we have another item that is currently "on" (colored), we need to shut if off
            if (currentlyPlayingItem != null)
            {
                currentlyPlayingItem.IsOn = false;
                if (currentlyPlayingItem.IsPlaying)
                {
                    currentlyPlayingItem.IsPlaying = false;
                    Pause();
                }

            }
            //Play current Item
            item.IsPlaying = true;
            item.IsOn = true;
            currentlyPlayingItem = item;

            //Actuall playing of the item
            //NOTE - it would make much more sense if we could use polymorphism here. The problem is that we cannot access the Player from Playable.
            if (item.Content.GetType() == PlayableType.Track)
            {
                Track track = (Track)(item.Content);
                Player.SetUri(track.FileUri);
                if (!IsPlaying) Play();
            }
            else if (item.Content.GetType() == PlayableType.Playlist)
            {
                PlayItem(((Playlist)item.Content).List[0]);
            }
        }
        #endregion

        #region Playlist editing functions
        public void IsTrackValid(Track newTrack)
        {
            if (newTrack.Title == "" || newTrack.Title == null) throw new Exception("Please type the new Track's title");
            if (newTrack.FileUri == "" || newTrack.FileUri == null) throw new Exception("Please paste the file address or browse a file from your computer.");
        }
        public void AddItem(Playable newTrack, Playlist target, int index = -1 /* -1 means add as last item */)
        {
            if (index == -1)
            {
                index = target.List.Count;
            }
            PlaylistItem newItem = new PlaylistItem(index + 1, newTrack);
            if (newItem.Content.Composer == null) newItem.Content.Composer = target.Composer;
            target.List.Insert(index, newItem);

        }
        internal void DeleteItems(List<PlaylistItem> targetItems, Playlist targetParent)
        {
            var ParentList = targetParent.List;

            //deleting target items from the list
            foreach(var item in targetItems)
            {
                ParentList.Remove(item);
            }

            //re - arrange indexing after deleting
            for (int i = 0; i < ParentList.Count; i++)
            {
                ParentList[i].Index = i + 1;
            }



        }
        #endregion
    }
}
