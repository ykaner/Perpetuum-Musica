using Player;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace PerpetuumMusica.Model
{
    public class Model
    {
        public AudioPlayer Player = new AudioPlayer();


        //public int currentlyPlayingIndex = -1; //-1 note nothing is played;
        private PlaylistItem currentlyPlayingItem { get; set; } 


        private bool isPlaying = true;
        private double location = 0;

        public Model()
        {
            Volume = 70;
            IsPlaying = false;
            //Player.SetUri(DemoData.sampleUri);
            Player.MediaEnded += Player_MediaEnded;
            //for testing
            //currentlyPlayingItem = DemoData.disneyPlaylist[0];
            //currentlyPlayingItem.IsPlaying = true;
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            if (MediaEnded != null)
            {
                MediaEnded(this, e);
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
        private double unmutedVolume;
        public double Volume
        {
            get { return Player.Volume; }
            set
            {
                Player.Volume = value;
            }
        }


        //Playing functions
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
    }
}
