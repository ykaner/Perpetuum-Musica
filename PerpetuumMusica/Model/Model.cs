using Player;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PerpetuumMusica.Model
{
    public class Model
    {
        public AudioPlayer Player = new AudioPlayer();

        
        //public int currentlyPlayingIndex = -1; //-1 note nothing is played;
        private PlaylistItem currentlyPlayingItem { get; set; } 


        private bool isPlaying = true;
        private double location = 0;
        private string sampleSongUri = @"C:\Users\ranha\Documents\GitHub\Perpetuum-Musica\ExampleAudioFiles\One Jump Ahead Lyrics.mp3";

        public Model()
        {
            Volume = 70;
            IsPlaying = false;
            Player.SetUri(sampleSongUri);

            //for testing
            currentlyPlayingItem = DemoData.DemoPlaylist[0];
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
                Player.Pause();
            else
                Player.Play();

            IsPlaying = !IsPlaying;
        }
        public void PlayAt(PlaylistItem target)
        {
            if (currentlyPlayingItem != null)
                currentlyPlayingItem.IsPlaying = false;

            target.IsPlaying = true;
            currentlyPlayingItem = target;
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
