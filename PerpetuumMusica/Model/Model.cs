using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Player;

namespace PerpetuumMusica.Model
{
    public class Model
    {
        public AudioPlayer Player = new AudioPlayer();
        public List<PlaylistItem> MainList = new List<PlaylistItem>()
        {
            new PlaylistItem(1, new Playable("The little mermaid: Original Broadway Cast Recording", new TimeSpan(1, 46, 0), 14, "Alan Menken", null)),
            new PlaylistItem(2, new Playable("Frozen (Original Motion Picture Soundtrack)", new TimeSpan(2, 9, 0), 21, "Kristen Anderson, Robert Lopez", null)),
            new PlaylistItem(3, new Playable("Aladdin Musical", new TimeSpan(1, 23, 0), 21, "Alan Menken", new List<PlaylistItem>()
            {
                new PlaylistItem(1, new Playable("One Jump ahead", new TimeSpan(1, 23, 0), 21, "Alan Menken", null) )
            }))
        };

        private bool isPlaying = true;
        private double location = 0;
        private string sampleSongUri = @"C:\Users\ranha\Documents\GitHub\Perpetuum-Musica\ExampleAudioFiles\One Jump Ahead Lyrics.mp3";

        public Model()
        {
            Volume = 70;
            IsPlaying = false;
            Player.SetUri(sampleSongUri);

            //for testing
            MainList[0].IsPlaying = true;
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



        public void TogglePlay()
        {
            if (IsPlaying)
                Player.Pause();
            else
                Player.Play();

            IsPlaying = !IsPlaying;
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
