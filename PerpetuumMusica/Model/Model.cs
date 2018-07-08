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
        public PlaylistItem CurrentlyPlayingItem { get; set; }
        public PlaylistItem ShowedItem { get; internal set; }
        public PlaylistItem RootItem { get; set; }

        //playing information
        private bool isPlaying = true;
        private double location = 0;

        #endregion

        #region Data loading functions
        private void Load()
        {
            ShowedItem = new PlaylistItem(new Playlist("All Playlists", null, new TimeSpan(), 0, "Various Artists", null));
            RootItem = ShowedItem;
            ((Playlist)ShowedItem.Content).List = new ObservableCollection<PlaylistItem>( DataBase.RetrievePlaylist(0));

            //var list = ((Playlist)ShowedItem.Content).List;
            //foreach(var item in list)
            //{
            //    LoadItem(item);
            //}




        }
        private void LoadItem(PlaylistItem target)
        {
            try //if it's a playlist, load the internal list
            {
                Playlist targetPlaylist = (Playlist)target.Content;
                if (targetPlaylist.List == null)
                {
                    //load internal list from dataBase (if not already loaded)
                    if (!targetPlaylist.ListLoaded)
                    {
                        targetPlaylist.List = new ObservableCollection<PlaylistItem>(DataBase.RetrievePlaylist(target.ID));
                        foreach (var item in targetPlaylist.List)
                        {
                            item.SetParent(target);
                        }
                        targetPlaylist.ListLoaded = true;
                    }
                }
            }
            catch
            {
                // if it's not a playlist 
                return;
            }

            
        }
        public void Open(PlaylistItem target)
        {
            ShowedItem = target;
            LoadItem(target);
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
            CurrentlyPlayingItem.IsPlaying = false;
            IsPlaying = false;
        }
        public void Play()
        {
            if (CurrentlyPlayingItem == null)
            {
                MessageBox.Show("Play \"showed\" Item (not implemented)");
                return;
            }
            else
                CurrentlyPlayingItem.IsPlaying = true;

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
            DataBase.IncreaseTimesPlayed(CurrentlyPlayingItem.Content);

            //currentlyPlayingItem = currentlyPlayingItem.Next;
            //Option 1 - play next in current list
            if (CurrentlyPlayingItem.Next != null)
            {
                PlayItem(CurrentlyPlayingItem.Next);
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
            if (CurrentlyPlayingItem != null)
            {
                CurrentlyPlayingItem.IsOn = false;
                if (CurrentlyPlayingItem.IsPlaying)
                {
                    CurrentlyPlayingItem.IsPlaying = false;
                    Pause();
                }

            }
            //Play current Item
            item.IsPlaying = true;
            item.IsOn = true;
            CurrentlyPlayingItem = item;

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
                Playlist list = (Playlist)(item.Content);
                LoadItem(item);
                if (list.List.Count > 0)
                {
                    PlayItem((list).List[0]);
                }
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

            int newId = -1;
            if (newTrack is Track)
            {
                newId = DataBase.InsertTrack((Track)newTrack, newItem);
            }
            else if (newTrack is Playlist)
            {
                newId = DataBase.InsertPlaylist((Playlist)newTrack, newItem);
            }

            newTrack.ID = newId;
        }
        public void DeleteItems(List<PlaylistItem> targetItems, Playlist targetParent)
        {
            var ParentList = targetParent.List;

            //deleting target items from the list
            foreach(var item in targetItems)
            {
                ParentList.Remove(item);

                DataBase.RemoveItem(item);
            }

            //re - arrange indexing after deleting
            for (int i = 0; i < ParentList.Count; i++)
            {
                ParentList[i].Index = i + 1;
            }


        }
        public void MoveItem(PlaylistItem targetItem, Playlist targetParent, int targetLocation)
        {
            targetParent.List.RemoveAt(targetItem.Index - 1);
            targetParent.List.Insert(targetLocation, targetItem);

            DataBase.Move(targetItem, targetLocation);

            //Update Indexes: 
            int minIndex, maxIndex;
            if (targetItem.Index - 1 < targetLocation)
            {
                minIndex = targetItem.Index - 1;
                maxIndex = targetLocation;
            }
            else
            {
                minIndex = targetLocation;
                maxIndex = targetItem.Index - 1;
            }
            for (int i = minIndex; i<=maxIndex; i++)
            {
                targetParent.List[i].Index = i + 1;
            }
        }
        public void ImportFolder(string url, Playlist target, int index = -1)
        {
            Playlist newPlaylist = CreatePlaylistFromFolder(url);
            AddItem(newPlaylist, target, index);
        }
        private Playlist CreatePlaylistFromFolder(string url)
        {
            ObservableCollection<PlaylistItem> list = new ObservableCollection<PlaylistItem>();

            //TODO - fill the list recurssively

            return new Playlist(1, "New Playlist", null, new TimeSpan(), 0, "unknown", list);
        }
        #endregion

    }
}
