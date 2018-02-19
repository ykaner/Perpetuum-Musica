using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerpetuumMusica.Model
{
    public class PlaylistItem
    {
        public int Index { get; set; }
        public bool IsPlaying { get; set; }
        public Playable Content { get; set; }

        public PlaylistItem(int index, Playable content)
        {
            Index = index;
            Content = content;
        }

        public PlaylistItem(Playable content)
        {
            Content = content;
            Index = 0;
        }
    }
}
