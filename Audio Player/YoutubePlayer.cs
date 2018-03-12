//trying to implement youtube palyer using webBrowser


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using System.Windows.Controls;

namespace Player
{
    class YoutubePlayer
    {

        private WebBrowser browser;

        private string GetYTVideoId(Uri uri)
        {
            string host = uri.Host;
            if(host != "www.youtube.com" && host != "www.youtu.be" 
                && host != "youtube.com" && host != "youtu.be")
            {
                return null;
            }

            /*var query = HttpUtility.ParseQueryString(uri.Query);

            var videoId = string.Empty;

            if (query.AllKeys.Contains("v"))
            {
                videoId = query["v"];
            }
            else
            {
                videoId = uri.Segments.Last();
            }
            
            return videoId;*/

            return "";
        }

        public Uri file {
            get {
                return file;
            }
            set {
                string videoId = GetYTVideoId(value);

                browser.Navigate("www.yotube.com/embed/" + videoId);
            }
        }

        public void pause()
        {
            return;
            dynamic doc = browser.Document;
            var vis = doc.getElementsByTagName("video");

            vis[0].pause();

        }

        public YoutubePlayer(WebBrowser wbr)
        {
            browser = wbr;
        }

        

    }
}
