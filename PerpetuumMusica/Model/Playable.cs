using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerpetuumMusica.Model
{
    public class Playable
    {
        public int ListIndex { get; set; }
        public string Title { get; set; }
        public TimeSpan Time { get; set; }
        public int TimesHeard { get; set; }
        public string Composer { get; set; }

        public Playable(int listIndex, string title, TimeSpan time, int timesHeard, string composer)
        {
            ListIndex = listIndex;
            Title = title;
            Time = time;
            TimesHeard = timesHeard;
            Composer = composer;
        }

    }
}
