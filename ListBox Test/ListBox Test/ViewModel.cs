using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ListBox_Test
{
    public class ViewModel : INotifyPropertyChanged
    {


        //public ICommand ShowCommand { get; set; }
        public string Name { get; set; }
        public ObservableCollection<string> Items { get; set; }
        private ICommand _showCommand;
        private ICommand _deleteCommand;
        public ICommand ShowCommand => _showCommand ?? (_showCommand = new Command(Show, CanShow));
        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new Command(Delete, CanShow));

        public void Show(object item)
        {
            MessageBox.Show(item.ToString());

        }
        public bool CanShow(object paramater)
        {
            if (paramater == null) return false;

            var itemsList = (paramater as ObservableCollection<object>).Cast<string>().ToList();

            if (itemsList.Count == 0) return false;
            else return true;
        }
        public bool CanDelete(object item)
        {
            return item != null;
        }
        
        public void Delete(object itemIndex)
        {
            int i = (int)itemIndex; 
            Items.RemoveAt(i);
            OnPropertyChanged("Items");
            
        }

        public ViewModel()
        {
            Name = "Me Willie";
            Items = new ObservableCollection<string> { "Klaymen", "Hoborg", "Klogg", "Willie Trombone" };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
