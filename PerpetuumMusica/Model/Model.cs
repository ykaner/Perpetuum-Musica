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
        public List<Playable> MainList = new List<Playable>()
        {
            new Playable(1, "The little mermaid: Original Broadway Cast Recording", new TimeSpan(1, 46, 0), 14, "Alan Menken", null),
            new Playable(2, "Frozen (Original Motion Picture Soundtrack)", new TimeSpan(2, 9, 0), 21, "Kristen Anderson, Robert Lopez", null),
            new Playable(3, "Aladdin Musical", new TimeSpan(1, 23, 0), 21, "Alan Menken", new List<Playable>()
            {
                new Playable(1, "One Jump ahead", new TimeSpan(1, 23, 0), 21, "Alan Menken", null)
            })
        };

        private bool isPlaying = true;
        private double location = 0;
        private string sampleSongUri = @"C:\Users\Shachar\Music\Soundtrack\Aladdin - Soundtrack Special Edition [DC Downloads]\03 - One Jump Ahead.mp3";

        public Model()
        {
            Volume = 70;
            IsPlaying = false;
            Player.SetUri(sampleSongUri);
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
