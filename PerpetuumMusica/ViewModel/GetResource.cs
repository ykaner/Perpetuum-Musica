using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PerpetuumMusica.ViewModel
{
    static public class GetResource
    {
        static public ImageSource Image(string name) { return (ImageSource)Application.Current.Resources[name]; }

        static public string TrackImage() { return "/View/Sources/DefaultImages/defaultTrackImage.png"; }
        static public string PlaylistImage() { return "/View/Sources/DefaultImages/defaultTrackImage.png"; }

    }
}
