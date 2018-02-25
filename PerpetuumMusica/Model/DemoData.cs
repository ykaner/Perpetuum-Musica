﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerpetuumMusica.Model
{
    public class DemoData
    {
        static public ObservableCollection<PlaylistItem> DemoPlaylist = new ObservableCollection<PlaylistItem>()
        {
            new PlaylistItem(1, new Playable("The little mermaid: Original Broadway Cast Recording", new TimeSpan(1, 46, 0), 14, "Alan Menken", null)),
            new PlaylistItem(2, new Playable("Frozen (Original Motion Picture Soundtrack)", new TimeSpan(2, 9, 0), 21, "Kristen Anderson, Robert Lopez", null)),
            new PlaylistItem(3, new Playable("Aladdin Musical", new TimeSpan(1, 23, 0), 21, "Alan Menken", new ObservableCollection<PlaylistItem>()
            {
                new PlaylistItem(1, new Playable("One Jump ahead", new TimeSpan(1, 23, 0), 21, "Alan Menken", null) )
            }))
        };

        static public PlaylistItem DemoItem = new PlaylistItem(new Playable("Disney", new TimeSpan(4, 38, 00), 13, "Various Artists", DemoPlaylist));
    }
}