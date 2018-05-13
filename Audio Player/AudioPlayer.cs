using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;

namespace Player
{

    public enum LinkType { local, youtube}

    public class AudioPlayer : LocalPlayer
    {
        private Uri file;
        public Uri File
        {
            get
            {
                return file;
            }
            set
            {
                this.file = value;
                this.uriType = getLinkType(file.OriginalString);
                if (uriType == LinkType.local)
                {
                    localPlayer.File = file;
                    System.Threading.SpinWait.SpinUntil(() => localPlayer.NaturalDuration.HasTimeSpan, 1 * 1000);
                    this.TotalSeconds = localPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                }
                else if (uriType == LinkType.youtube)
                {
                    var video = LinkResolver.ResolveLink(file.OriginalString);
                    this.TotalSeconds = video.Seconds;
                    youtubePlayer.current = video;
                    string tmpFile = youtubePlayer.StartStreaming();
                    localPlayer.SetUri(tmpFile);
                }
            }
        }
        private LinkType uriType;
        private double TotalSeconds { get; set; }

        private LocalPlayer localPlayer;
        private YTPlayer youtubePlayer;

        public void SetUri(string file)
        {
            this.File = new Uri(file);
        }

        public AudioPlayer()
        {
            localPlayer = new LocalPlayer();
            youtubePlayer = new YTPlayer();
        }

        public LinkType getLinkType(string uri)
        {
            Regex YoutubeLinkRegex = new Regex("(?:.+?)?(?:\\/v\\/|watch\\/|\\?v=|\\&v=|youtu\\.be\\/|\\/v=|^youtu\\.be\\/)([a-zA-Z0-9_-]{11})+");
            
            if (YoutubeLinkRegex.Match(uri).Success)
                return LinkType.youtube;
            else
                return LinkType.local;
        }
        public void Play()
        {
            localPlayer.Play();
        }

        public void Pause()
        {
            localPlayer.Pause();
        }

        public void Stop()
        {
            localPlayer.Stop();
        }

        public string GetProgressTime()
        {
            return localPlayer.GetProgressTime();
        }

        public double GetProgressPercent()
        {
            return localPlayer.Position.Seconds / TotalSeconds;
        }

        public bool SetTime(TimeSpan time)
        {
            return localPlayer.SetTime(time);
        }

        public bool SetTime(double percent)
        {
            return localPlayer.SetTime(percent);
        }

    }
}
