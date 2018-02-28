using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PerpetuumMusica.ViewModel;

namespace PerpetuumMusica.Model
{
    public class DemoData
    {
        static private ImageSource Image(string name) { return ViewModel.ViewModel.Image(name); }

        static public ObservableCollection<PlaylistItem> disneyPlaylist = new ObservableCollection<PlaylistItem>()
        {
            new PlaylistItem(1, new Playable("The little mermaid: Original Broadway Cast Recording", Image("littleMermaid"), new TimeSpan(1, 46, 0), 14, "Alan Menken")),
            new PlaylistItem(2, new Playable("Frozen (Original Motion Picture Soundtrack)", Image("frozen"),  new TimeSpan(2, 9, 0), 21, "Kristen Anderson, Robert Lopez")),
            new PlaylistItem(3, new Playable("Aladdin Musical", Image("aladdin"), new TimeSpan(1, 23, 0), 21, "Alan Menken"), new ObservableCollection<PlaylistItem>()
            {
                new PlaylistItem(1, new Playable("One Jump ahead", Image("aladdin"),  new TimeSpan(1, 23, 0), 21, "Alan Menken") )
            })
        };

        static public PlaylistItem Disney = new PlaylistItem(1, new Playable("Disney", Image("disney"), new TimeSpan(4, 38, 00), 13, "Various Artists"), disneyPlaylist);

        static public PlaylistItem musicals = new PlaylistItem(1, new Playable("Musicals"), new ObservableCollection<PlaylistItem>()
        {
            new PlaylistItem(1, new Playable("Movies"), new ObservableCollection<PlaylistItem>() { Disney })
        });



        static public void SetParents(PlaylistItem playlist)
        {
            if (playlist.List == null) return;

            foreach(PlaylistItem item in playlist.List)
            {
                item.SetParent(playlist);
                SetParents(item);
            }
        }

    }
}
