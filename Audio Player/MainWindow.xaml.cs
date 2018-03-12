using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private AudioPlayer audioPlayer;
        private YoutubePlayer YTPlayer;
        
        public MainWindow()
        {
            InitializeComponent();

            audioPlayer = new AudioPlayer();
            YTPlayer = new YoutubePlayer(wbBrwsr);

            audioPlayer.SetUri("http://s71.podbean.com/pb/2aba70e5c3fff66c5ec21b96971cfd95/5a664d03/data3/fs60/688336/uploads/EP159_Algorithmic_Trading_Fixed.mp3");

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timerTick;
            timer.Start();

        }

        void timerTick(object sender, EventArgs e)
        {
            if (audioPlayer.Source != null)
            {
                lblStatus.Content = audioPlayer.GetProgressTime();
                progBar.Value = audioPlayer.GetProgressPercent();
            }
            else
            {
                lblStatus.Content = "file not set";
                progBar.Visibility = Visibility.Hidden;
            }
        }

        public void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            audioPlayer.Play();
        }

        public void btnPause_Click(object sender, RoutedEventArgs e)
        {
            audioPlayer.Pause();
        }

        public void btnStop_Click(object sender, RoutedEventArgs e)
        {
            audioPlayer.Stop();
        }

        public void btnJump_Click(object sender, RoutedEventArgs e)
        {
            audioPlayer.SetTime(0.3);
        }



        public void youtube_play(object sender, RoutedEventArgs e)
        {

            //wbBrwsr.Navigate("C:/Users/ykane/Documents/scripts/youtube.html");
            wbBrwsr.Navigate("https://www.youtube.com/embed/ASSOQDQvVLU?autoplay=1");
            //string curDir = Directory.GetCurrentDirectory();
            //wbBrwsr.Navigate(new Uri(String.Format("file:///{0}/YTPage.html", curDir)));
        }

        public void youtube_pause(object sender, RoutedEventArgs e)
        {
            YTPlayer.pause();
        }

    }
}
