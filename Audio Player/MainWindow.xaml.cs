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

            //audioPlayer.SetUri(@"C:\Users\ykane\Downloads\♫ Aladdin - One Jump Ahead Lyrics ♫.mp3");
            audioPlayer.SetUri("https://www.youtube.com/watch?v=fcTC7RkmHac");

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timerTick;
            timer.Start();

        }

        void timerTick(object sender, EventArgs e)
        {
            if (audioPlayer.File != null)
            {
                lblStatus.Content = audioPlayer.GetProgressTime();
                progBar.Value = audioPlayer.GetProgressPercent() * 100;
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
            audioPlayer.SetTime(30);
        }



        public void youtube_play(object sender, RoutedEventArgs e)
        {

            //wbBrwsr.Navigate("C:/Users/ykane/Documents/scripts/youtube.html");
            //wbBrwsr.Navigate("https://www.youtube.com/embed/ASSOQDQvVLU?autoplay=1");
            wbBrwsr.Navigate("https://r1---sn-h5jic5-ua8e.googlevideo.com/videoplayback?source=youtube&pl=16&fvip=3&ei=zPrSWrmqF8jC1gKIuqJw&itag=251&lmt=1466922548431373&ipbits=0&dur=327.261&expire=1523797804&initcwndbps=1101250&requiressl=yes&mime=audio%2Fwebm&gir=yes&key=yt6&signature=AC43DB96B1369E4ACEC4C33D2CD7238AB1B3B13D.92DE9A0111A01A25ABC235AE3A22320B3021583C&ip=132.74.208.118&ms=au%2Crdu&mt=1523776125&sparams=clen%2Cdur%2Cei%2Cgir%2Cid%2Cinitcwndbps%2Cip%2Cipbits%2Citag%2Ckeepalive%2Clmt%2Cmime%2Cmm%2Cmn%2Cms%2Cmv%2Cpl%2Crequiressl%2Csource%2Cexpire&mv=m&id=o-ACKVGeKZ8Ui_t8Nj69D1pryeqIkySl8lMS9DRmc5NXfv&keepalive=yes&mm=31%2C29&mn=sn-h5jic5-ua8e%2Csn-5hnedn7e&clen=4075606&c=WEB&ratebypass=yes");
            //string curDir = Directory.GetCurrentDirectory();
            //wbBrwsr.Navigate(new Uri(String.Format("file:///{0}/YTPage.html", curDir)));
        }

        public void youtube_pause(object sender, RoutedEventArgs e)
        {
            YTPlayer.pause();
        }

    }
}
