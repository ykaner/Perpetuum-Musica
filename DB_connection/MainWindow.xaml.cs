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


namespace DB_connection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DB_conn connection = new DB_conn();
        public MainWindow()
        {
            InitializeComponent();
        }

        public void btnSelect_click(object sender, RoutedEventArgs e)
        {
            List<string>[] result = connection.Select();
            return;
        }

        private void btnInsertList_Click(object sender, RoutedEventArgs e)
        {
            connection.InsertPlaylist(0, 5, new Duration(new TimeSpan(0, 0, 0)), "Modern",
                "satoshi");
        }
    }
}
