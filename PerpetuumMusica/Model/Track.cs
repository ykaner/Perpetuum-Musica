using PerpetuumMusica.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PerpetuumMusica.Model
{
    public class Track : Playable
    {
        public String FileUri { get; set; }


        public Track()
        {

        }
        public Track(int id, string title, String fileUri, ImageSource image, TimeSpan time, int timesHeard, string composer) :
            base(id, title, image, time, timesHeard, composer)
        {
            FileUri = fileUri;
        }
        public Track(string title, string fileUri, ImageSource image, string composer) : base(title)
        {
            Title = title;
            FileUri = fileUri;
            Image = image;
            Composer = composer;

            //TODO - 
            //Find the other properties using the url, or throw an Exception if the url is not accessible 
        }
        public Track(string title, string fileUri, ImageSource image, TimeSpan time, int timesHeard, string composer) : this(0, title, fileUri, image, time, timesHeard, composer) { }


        public override PlayableType GetType() { return PlayableType.Track; }
        public override ImageSource DefaultImage => GetResource.Image("defaultTrackImage");


        public override string ToString()
        {
            return Title;
        }

    }
}
