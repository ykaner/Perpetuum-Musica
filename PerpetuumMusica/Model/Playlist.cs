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
        public Playlist(string title, ImageSource image, TimeSpan time, int timesHeard, string composer, ObservableCollection<PlaylistItem> list) : base(title, image, time, timesHeard, composer, list) { }

        public override PlayableType GetType()
        {
            return PlayableType.Playlist;
        }
    }
}
