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
        private int unmutedVolume;
        public int Volume { get; set; }

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
