using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PerpetuumMusica.ViewModel
{
    public class HomePageViewModel
    {
        private ViewModel ViewModel;

        public HomePageViewModel(ViewModel vm)
        {
            ViewModel = vm;
        }

        private Command _GoToPlaylistCommand;
        public Command GoToPlaylistCommand => _GoToPlaylistCommand ?? (_GoToPlaylistCommand = new Command(OpenMainPlaylist));

        private int MainPlaylistPageIndex = 2;

        private void OpenMainPlaylist(object obj)
        {
            ViewModel.CurrentPageIndex = MainPlaylistPageIndex;
        }


    }
}
