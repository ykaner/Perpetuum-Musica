using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Player
{
    public class AudioPlayer : MediaPlayer
    {

        private Uri file;

        public AudioPlayer() : base()
        {
            
        }

        public bool SetUri(String file)
        {
            try
            {
                this.file = new Uri(file);
                base.Open(this.file);
            }
            catch (Exception e){
                return false;
            }
            return true;
        }

        public new void Play()
        {
            base.Play();
        }

        public new void Pause()
        {
            base.Pause();
        }

        public new void Stop()
        {
            base.Stop();
            base.Close();
        }

        public String GetProgressTime()
        {
            return this.Position.ToString(@"mm\:ss") + " / " + this.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
        }

        public double GetProgressPercent()
        {
            return this.Position.TotalMilliseconds / this.NaturalDuration.TimeSpan.TotalMilliseconds * 100;
        }

        public bool SetTime(TimeSpan time)
        {
            if (this.NaturalDuration.TimeSpan < time)
                return false;
            else
            {
                this.Position = time;
                return true;
            }
        }

        public bool SetTime(double percents)
        {
            if(percents > 1 || percents < 0)
            {
                return false;
            }
            else
            {
                this.Position = new TimeSpan((long)(this.NaturalDuration.TimeSpan.Ticks * percents));
                return true;
            }
        }

    }
}
