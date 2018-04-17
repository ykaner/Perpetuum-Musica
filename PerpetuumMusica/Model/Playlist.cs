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
    public class Playlist : Playable
    {
        public ObservableCollection<PlaylistItem> List { get; set; }

        public bool ListLoaded { get; set; } //true iff the list is loaded from the database

        public Playlist(int id, string title, ImageSource image, TimeSpan time, int timesHeard, string composer, ObservableCollection<PlaylistItem> list) :
            base(id, title, image, time, timesHeard, composer)
        {
            List = list;
            ListLoaded = false;
            //List.CollectionChanged += List_CollectionChanged;
            //registerOnSelectionChanged();
        }
        /*
        private void registerOnSelectionChanged()
        {
            foreach ( PlaylistItem item in List )
            {
                item.PropertyChanged += Item_PropertyChanged;
            }
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ( e.PropertyName == "IsSelected" )
            {
                _selectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void List_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private event EventHandler _selectionChanged;
        public event EventHandler SelectionChanged
        {
            add { _selectionChanged += value; }
            remove { _selectionChanged -= value; }
        }
        */

        public Playlist(string title, ImageSource image, TimeSpan time, int timesHeard, string composer, ObservableCollection<PlaylistItem> list) : this(0, title, image, time, timesHeard, composer, list) { }

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


        public override PlayableType GetType() { return PlayableType.Playlist; }
        public override string DefaultImage => GetResource.PlaylistImage();

    }
}
