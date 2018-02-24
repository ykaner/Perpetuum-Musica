﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerpetuumMusica.Model
{
    public class PlaylistItem : INotifyPropertyChanged
    {
        public int Index { get; set; }
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
