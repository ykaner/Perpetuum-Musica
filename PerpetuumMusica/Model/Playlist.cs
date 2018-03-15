using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PerpetuumMusica.Model
{
    public class Playlist : Playable
    {
        public ObservableCollection<PlaylistItem> List { get; set; }

        public Playlist(int id, string title, ImageSource image, TimeSpan time, int timesHeard, string composer, ObservableCollection<PlaylistItem> list) :
            base(id, title, image, time, timesHeard, composer)
        {
            List = list;
        }

        private int CurrentItemIndex { get; set; } //Index of item that is currently on/playing
        //return the current playing item of _this_ playlist
        public PlaylistItem Current
        {
            get
            {
                return List[CurrentItemIndex];
            }
            set
            {
                if (value.Parent.Content != this) throw new Exception("Cannot asign item that is not in the same list");

                CurrentItemIndex = value.Index - 1;
            }
        }
        public PlaylistItem Next
        {
            get
            {
                try
                {
                    return List[CurrentItemIndex + 1];
                }
                catch
                {
                    return null;
                }
                
            }
        }


        public override PlayableType GetType()
        {
            return PlayableType.Playlist;
        }
    }
}
