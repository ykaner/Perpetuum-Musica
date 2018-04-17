using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerpetuumMusica.Model
{
    public class PlaylistItem : INotifyPropertyChanged
    {
        public int ID { get; }
        private int _index;
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                OnPropertyChanged("Index");
            }
        }
        private Playable _content;
        public Playable Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }
        //public ObservableCollection<PlaylistItem> List { get; set; }
        public List<int> Path_id { get; set; }
        public List<PlaylistItem> Path { get; set; }
        public PlaylistItem Parent
        {
            get
            {
                if (Path.Count == 0) return null;
                else
                    return Path[Path.Count - 1];
            }
        }
        public Playlist ParentPlaylist
        {
            get
            {
                return (Playlist)(Parent.Content);
            }
        }


        private bool _IsPlaying;
        public bool IsPlaying
        {
            get
            {
                return _IsPlaying;
            }
            set
            {
                //recursive set of IsPlaying property
                if (_IsPlaying == value) return;

                _IsPlaying = value;

                if (Parent != null)
                {
                    Parent.IsPlaying = value;
                }
                OnPropertyChanged("IsPlaying");
            }
        }
        private bool _IsOn;  //Detemind whether "play" will play this by default.
        public bool IsOn
        {
            get
            {
                return _IsOn;
            }
            set
            {
                //recursive set of IsOn property
                //stoping condition - if we are already marked correctly
                if (_IsOn == value) return;

                //mark this
                _IsOn = value;

                //if we need to update the currently playing item of the parent list
                if (value && Parent != null)
                {
                    //update CurrentlyPlaying
                    ((Playlist)Parent.Content).Current = this;
                }

                if (Parent != null)
                {
                    Parent.IsOn = value;
                }
                OnPropertyChanged("IsOn");
            }
        }
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }


        //Next after me, in playlist "Parent"
        public PlaylistItem Next
        {
            get
            {
                return ((Playlist)Parent.Content).Next;
            }
        }

        public PlaylistItem(int id, int index, Playable content/*, ObservableCollection<PlaylistItem> list = null*/)
        {
            this.ID = id;
            Index = index;
            Content = content;
            //List = list;
            Path = new List<PlaylistItem>();
            Path_id = new List<int>();
        }
        public PlaylistItem(int index, Playable content) : this(0, index, content) { }

        public void SetParent(PlaylistItem parent)
        {
            //set id of parent
            Path_id = new List<int>(parent.Path_id);
            Path_id.Add(parent.ID);

            //set parent
            Path = new List<PlaylistItem>(parent.Path);
            Path.Add(parent);
        }

        public PlaylistItem(Playable content)
        {
            Content = content;
            Index = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override string ToString()
        {
            return Content.Title;
        }
    }
}
