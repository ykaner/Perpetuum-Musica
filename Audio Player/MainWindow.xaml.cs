using System;
using System.Collections.Generic;
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

        private AudioPlayer audioPlayer = new AudioPlayer();

        public MainWindow()
        {
            InitializeComponent();

            audioPlayer.SetUri("C:/Users/ykane/Downloads/Pirates of the Caribean piano.mp3");

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
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




        public void youtube_play(object sender, RoutedEventArgs e)
        {
            //wbSample.Navigate("C:/Users/ykane/Documents/scripts/youtube.html");
            wbSample.Navigate("https://www.youtube.com/watch?v=sNhhvQGsMEc&autoplay=1");
            //wbSample.Navigate("https://www.youtube.com");
        }
    }
}
