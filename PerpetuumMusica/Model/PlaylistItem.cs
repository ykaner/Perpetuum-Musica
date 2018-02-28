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
        public ObservableCollection<PlaylistItem> List { get; set; }
        public List<PlaylistItem> Path { get; set; }

        private bool _IsPlaying;
        public bool IsPlaying
        {
            get
            {
                return _IsPlaying;
            }
            set
            {
                _IsPlaying = value;
                OnPropertyChanged("IsPlaying");
            }
        }

        public PlaylistItem(int index, Playable content, ObservableCollection<PlaylistItem> list = null)
        {
            Index = index;
            Content = content;
            List = list;
            Path = new List<PlaylistItem>();
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
