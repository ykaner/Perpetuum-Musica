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
            Player.SetUri(DemoData.sampleUri);

            //for testing
            //currentlyPlayingItem = DemoData.disneyPlaylist[0];
            //currentlyPlayingItem.IsPlaying = true;
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
                Player.Pause();
                currentlyPlayingItem.IsPlaying = false;
            }
            else
            {
                if (currentlyPlayingItem == null)
                {
                    MessageBox.Show("Play \"showed\" Item");
                    return;
                }
                else
                    currentlyPlayingItem.IsPlaying = true;

                Player.Play();
            }


            IsPlaying = !IsPlaying;
        }
        public void TogglePlayItem(PlaylistItem target)
        {
            //if we toggle to currently played item, we need to pause it
            if (target.IsOn == true)
            {
                target.IsPlaying = !target.IsPlaying;
                TogglePlay();
                return;
            }

            //if we have another item that is currently "on"(colored), we need to shut if off
            if (currentlyPlayingItem != null)
            {
                currentlyPlayingItem.IsOn = false;
                if (currentlyPlayingItem.IsPlaying)
                {
                    currentlyPlayingItem.IsPlaying = false;
                    TogglePlay();
                }

            }

            //Play current Item
            target.IsPlaying = true;
            target.IsOn = true;
            currentlyPlayingItem = target;
            PlayItem(target);
        }
        private void PlayItem(PlaylistItem item)
        {
            //MessageBox.Show("play item " + item.Content.Title);
            if (item.Content.GetType() == PlayableType.Track)
            {
                Track track = (Track)(item.Content);
                Player.SetUri(track.FileUri);
            }
            TogglePlay();
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
