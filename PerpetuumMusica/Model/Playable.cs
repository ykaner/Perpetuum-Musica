using PerpetuumMusica.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PerpetuumMusica.Model
{
    public enum PlayableType { Track, Playlist}

    public abstract class Playable
    {
        public int ID { get; set; }
        public string Title { get; set; }
        private ImageSource _image;
        public ImageSource Image
        {
            get { return _image ?? (DefaultImage); }
            set { _image = value; }
        }
        public TimeSpan Time { get; set; }
        public int TimesHeard { get; set; }
        public string Composer { get; set; }
        //public ObservableCollection<PlaylistItem> List { get; set; }


        public Playable() { }
        public Playable(int id, string title, ImageSource image, TimeSpan time, int timesHeard, string composer)
        {
            //ListIndex = listIndex;
            this.ID = id;
            Title = title;
            Time = time;
            TimesHeard = timesHeard;
            Composer = composer;
            //List = list;
            Image = image;
        }
        public Playable(string title)
        {
            Title = title;
            Time = new TimeSpan(0);
            TimesHeard = 0;
            Composer = "Various Artist";
        }

        public abstract PlayableType GetType();
        public abstract ImageSource DefaultImage { get; }

    }
}
