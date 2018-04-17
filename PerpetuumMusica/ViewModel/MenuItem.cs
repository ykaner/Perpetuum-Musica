using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace PerpetuumMusica.ViewModel
{
    public class MenuItem
    {
        
        //public string Key { get; set; }
        public string Name { get; set; }
        public ImageSource Icon { get; set; }
        public ICommand Command { get; set; }
        public List<MenuItem> SubItems { get; set; }

        public MenuItem(string name = "untitled", ImageSource icon = null, ICommand command = null, List<MenuItem> subItems = null)
        {
            this.Name = name;
            this.Icon = icon;
            this.Command = command;
            this.SubItems = subItems;
            //this.Key = key;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
