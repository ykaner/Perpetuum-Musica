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

        public double LocationPercentage
        {
            get
            {
                return GetProgressPercent();
            }
            set
            {
                SetTime(value);
            }
        }

        public String GetProgressTime()
        {
            String res = this.Position.ToString(@"mm\:ss") + " / ";
            if (!this.NaturalDuration.HasTimeSpan)
            {
                return "";
            }
            if (this.NaturalDuration.TimeSpan.Hours != 0)
            {
                res += this.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");
            }
            else if(this.NaturalDuration.TimeSpan.Minutes != 0){
                res += this.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
            }
            else
            {
                res += this.NaturalDuration.TimeSpan.ToString(@"ss");
            }
            return res;
        }

        public double GetProgressPercent()
        {
            if (!this.NaturalDuration.HasTimeSpan)
            {
                return 0;
            }
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
            percents /= 100;
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
