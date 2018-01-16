using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PerpetuumMusica.Model
{
    public class Model
    {
        private bool isPlaying = true;
        private double location = 0;

        public Model()
        {
            Volume = 70;
            IsPlaying = false;
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
