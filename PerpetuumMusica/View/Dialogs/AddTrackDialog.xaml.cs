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
using System.Windows.Shapes;
using PerpetuumMusica.ViewModel;
using PerpetuumMusica.View.CustomControls;

namespace PerpetuumMusica.View
{
    /// <summary>
    /// Interaction logic for AddTrackDialogBox.xaml
    /// </summary>
    /// 

    public class GrabableWindow : Window
    {
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }
   
    }

    public partial class AddTrackDialog : GrabableWindow
    {
        public AddTrackDialog(AddTrackDialogViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
              

        public void Close(object sender, RoutedEventArgs e)
        {

        }
    }
}
