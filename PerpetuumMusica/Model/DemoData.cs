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
    static public class DemoData
    {
        static private string directory = @"C:\Users\ranha\Documents\GitHub\Perpetuum-Musica\ExampleAudioFiles\";
        static private string one_jump_ahead = "One Jump Ahead Lyrics.mp3";
        static private string LetItGo = "Frozen Let It Go.mp3";
        static private string littleMermaid = "The Little Mermaid.mp3";


        static public string sampleUri
        {
            get { return directory + one_jump_ahead; }
        }

        static private string uri(string name) { return directory + name; }

        static private ImageSource Image(string name) { return ViewModel.ViewModel.Image(name); }

        static public ObservableCollection<PlaylistItem> disneyPlaylist = new ObservableCollection<PlaylistItem>()
        {
            new PlaylistItem(1, new Track("The little mermaid: Original Broadway Cast Recording", uri(littleMermaid), Image("littleMermaid") , new TimeSpan(1, 46, 0), 14, "Alan Menken")),
            new PlaylistItem(2, new Track("Frozen (Original Motion Picture Soundtrack)", uri(LetItGo), Image("frozen"),  new TimeSpan(2, 9, 0), 21, "Kristen Anderson, Robert Lopez")),
            new PlaylistItem(3, new Playlist("Aladdin Musical", Image("aladdin"), new TimeSpan(1, 23, 0), 21, "Alan Menken", new ObservableCollection<PlaylistItem>()
            {
                new PlaylistItem(1, new Track("One Jump ahead", uri("One Jump Ahead Lyrics.mp3"), Image("aladdin"),  new TimeSpan(1, 23, 0), 21, "Alan Menken") )
            }))
        };

        static public PlaylistItem Disney = new PlaylistItem(1, new Playlist("Disney", Image("disney"), new TimeSpan(4, 38, 00), 13, "Various Artists", disneyPlaylist));

        static public PlaylistItem musicals = new PlaylistItem(1, new Playlist("Musicals", null, new TimeSpan(0), 1, "Various Artists", new ObservableCollection<PlaylistItem>()
        {
            new PlaylistItem(1, new Playlist("Movies", null, new TimeSpan(0), 1, "Various Artists", new ObservableCollection<PlaylistItem>() { Disney }))
        }));



        static public void SetParents(PlaylistItem playlist)
        {
            if (playlist.Content.List == null) return;

            foreach(PlaylistItem item in playlist.Content.List)
            {
                item.SetParent(playlist);
                SetParents(item);
            }
        }

    }
}
