﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerpetuumMusica.View;
using PerpetuumMusica.Model;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace PerpetuumMusica.ViewModel
{
    public class AddTrackDialogViewModel : PlayableDialogViewModel
    {        
        public AddTrackDialogViewModel(Model.Model model)
        {
            Model = model;
        }

        public Track ShowDialog()
        {
            ResetFields();
            dialog = new AddTrackDialog(this);

            dialog.ShowDialog();
            if (IsOk)
            {
                GetImageFromPath();
                Track track = (Track)Playable;
                track.FileUri = track.FileUri.Replace("\\", "/");
                return track;
            }
            else
                return null;
        }
 
        protected override void ResetFields()
        {
            base.ResetFields();
            this.Playable = new Track();
        }
        
        private Command _ChooseFileCommand;
        public Command ChooseFileCommand => _ChooseFileCommand ?? (_ChooseFileCommand = new Command(ChooseFile));
        
        private void ChooseFile(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                ((Track)Playable).FileUri = fileName;

                OnPropertyChanged("Playable");
            }
            else
                return;
        }

        protected override void CheckValidity()
        {
            Model.IsTrackValid((Track)Playable);
        }
    }
}
