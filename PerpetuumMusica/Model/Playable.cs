using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PerpetuumMusica.Model
{
    public class Playable
    {
        //public int ListIndex { get; set; }
        public string Title { get; set; }
        public BitmapImage Image { get; set; }
        public TimeSpan Time { get; set; }
        public int TimesHeard { get; set; }
        public string Composer { get; set; }
        public ObservableCollection<PlaylistItem> List { get; set; }

        public Playable(string title, BitmapImage image, TimeSpan time, int timesHeard, string composer, ObservableCollection<PlaylistItem> list)
        {
            //ListIndex = listIndex;
            Title = title;
            Time = time;
            TimesHeard = timesHeard;
            Composer = composer;
            List = list;
        }

    }
}
