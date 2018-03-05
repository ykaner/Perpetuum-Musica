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
        public int Index { get; set; }
        public Playable Content { get; set; }
        //public ObservableCollection<PlaylistItem> List { get; set; }
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
                if (_IsOn == value) return;

                _IsOn = value;
                if (value && Parent != null)
                {
                    //update CurrentlyPlaying
                    Parent.Current = this;
                }

                if (Parent != null)
                {
                    Parent.IsOn = value;
                }
                OnPropertyChanged("IsOn");
            }
        }
        
        private int CurrentItemIndex { get; set; } //Index of item that is currently on/playing
        public PlaylistItem Current
        {
            get
            {
                return Content.List[CurrentItemIndex];
            }
            set
            {
                if (value.Parent != this) throw new Exception("Cannot asign item that is not in the same list");

                CurrentItemIndex = value.Index - 1;
            }
        }
        public PlaylistItem Next
        {
            get
            {
                try
                {
                    return Parent.Content.List[Parent.CurrentItemIndex + 1];
                }
                catch
                {
                    return null;
                }
            }
        }
        public PlaylistItem Previous
        {
            get
            {
                try
                {
                    return Parent.Content.List[CurrentItemIndex - 1];
                }
                catch
                {
                    return null;
                }
            }
        }


        public PlaylistItem(int index, Playable content/*, ObservableCollection<PlaylistItem> list = null*/)
        {
            Index = index;
            Content = content;
            //List = list;
            Path = new List<PlaylistItem>();
            CurrentItemIndex = 0;
        }

        public void SetParent(PlaylistItem parent)
        {
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
