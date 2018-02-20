using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ListBox_Test
{
    public class ViewModel
    {
        public ICommand ShowCommand { get; set; }
        public string Name { get; set; }
        public List<string> Items { get; set; }

        public ViewModel()
        {
            Name = "Me Willie";
            Items = new List<string> { "Klaymen", "Hoborg", "Klogg", "Willie Trombone" };
            ShowCommand = new ShowCommand();
        }
    }
}
