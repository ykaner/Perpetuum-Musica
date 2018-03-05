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

        public Track(string title, String fileUri, ImageSource image, TimeSpan time, int timesHeard, string composer) : base(title, image, time, timesHeard, composer)
        {
            FileUri = fileUri;
        }

        public override PlayableType GetType()
        {
            return PlayableType.Track;
        }

    }
}
