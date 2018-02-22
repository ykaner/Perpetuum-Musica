using Player;
using System;
using System.Collections.Generic;

namespace PerpetuumMusica.Model
{
    public class Model
    {
        public AudioPlayer Player = new AudioPlayer();

        public List<PlaylistItem> Playlist = new List<PlaylistItem>()
        {
            new PlaylistItem(1, new Playable("The little mermaid: Original Broadway Cast Recording", new TimeSpan(1, 46, 0), 14, "Alan Menken", null)),
            new PlaylistItem(2, new Playable("Frozen (Original Motion Picture Soundtrack)", new TimeSpan(2, 9, 0), 21, "Kristen Anderson, Robert Lopez", null)),
            new PlaylistItem(3, new Playable("Aladdin Musical", new TimeSpan(1, 23, 0), 21, "Alan Menken", new List<PlaylistItem>()
            {
                new PlaylistItem(1, new Playable("One Jump ahead", new TimeSpan(1, 23, 0), 21, "Alan Menken", null) )
            }))
        };
        private int currentlyPlayingIndex = -1; //-1 note nothing is played;

        private bool isPlaying = true;
        private double location = 0;
        private string sampleSongUri = @"C:\Users\ranha\Documents\GitHub\Perpetuum-Musica\ExampleAudioFiles\One Jump Ahead Lyrics.mp3";

        public Model()
        {
            Volume = 70;
            IsPlaying = false;
            Player.SetUri(sampleSongUri);

            //for testing
            PlayAt(0);
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
        public void PlayAt(int i)
        {
            if (currentlyPlayingIndex != -1)
                Playlist[currentlyPlayingIndex].IsPlaying = false;

            Playlist[i].IsPlaying = true;
            currentlyPlayingIndex = i;
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
