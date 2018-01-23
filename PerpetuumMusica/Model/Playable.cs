using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerpetuumMusica.Model
{
    public class Playable
    {
        public string Title { get; set; }
        public TimeSpan Time { get; set; }
        public int TimesHeard { get; set; }
        public string Composer { get; set; }

        public Playable(string title, TimeSpan time, int timesHeard, string composer)
        {
            Title = title;
            Time = time;
            TimesHeard = timesHeard;
            Composer = composer;
        }

    }
}
