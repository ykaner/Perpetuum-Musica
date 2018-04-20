using PerpetuumMusica.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerpetuumMusica.View;

namespace PerpetuumMusica.ViewModel
{
    public class AddEmptyPlaylistDialogViewModel : PlayableDialogViewModel
    {
        public AddEmptyPlaylistDialogViewModel(Model.Model model) { Model = model; }

        public Playlist ShowDialog()
        {
            ResetFields();

            dialog = new AddEmptyPlaylist(this);

            dialog.ShowDialog();
            if (IsOk)
            {
                GetImageFromPath();
                ((Playlist)Playable).List = new System.Collections.ObjectModel.ObservableCollection<PlaylistItem>();
                return (Playlist)Playable;
            }
            else
                return null;
        }

        protected override void ResetFields()
        {
            base.ResetFields();
            this.Playable = new Playlist();
        }

        protected override void CheckValidity()
        {
            
        }
    }
}
