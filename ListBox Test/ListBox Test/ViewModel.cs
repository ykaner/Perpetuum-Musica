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
        private ICommand _showOneCommand;
        private ICommand _helloWorldCOmmand;
        private ICommand _addCommand;
        public ICommand ShowCommand => _showCommand ?? (_showCommand = new Command(Show, CanShow));
        public ICommand ShowOneCommand => _showOneCommand ?? (_showOneCommand = new Command(ShowOne));
        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new Command(Delete, CanShow));
        public ICommand AddCommand => _addCommand ?? (_addCommand = new Command(Insert));

        public ICommand HelloWorldCommand => _helloWorldCOmmand ?? (_helloWorldCOmmand = new Command(HelloWorld));

        public int SelectedIndex { get; set; }

        private List<string> objectToList(object param)
        {
            System.Collections.IList list = (System.Collections.IList)param;
            var collection = list.Cast<string>();
            return collection.ToList<string>();
        }

        public void Show(object param)
        {
            var items = objectToList(param);

            string output = "Selected: \n";
            foreach (string item in items)
            {
                output += item + "\n"; 
            }
            MessageBox.Show(output);
        }
        public void ShowOne(object param)
        {
            MessageBox.Show(param.ToString());
        }
        public bool CanShow(object paramater)
        {
            if (paramater == null) return false;

            System.Collections.IList items = (System.Collections.IList)paramater;
            var collection = items.Cast<string>();

            if (items.Count == 0) return false;
            else return true;
        }
        
        public void Delete(object param)
        {
            var items = objectToList(param);

            foreach (string item in items)
            {
                Items.Remove(item);
            }
            
        }
        public void Insert(object param)
        {
            AddDialog dialog = new AddDialog();
            dialog.ShowDialog();
            int i = (int)param;
            if (i == -1)
            {
                Items.Add("untitled");
                SelectedIndex = Items.Count - 1;
            }
            else
            {
                Items.Insert(i + 1, "untitled");
                SelectedIndex = i + 1;
            }
            OnPropertyChanged("SelectedIndex");


        }
        public void HelloWorld(object param)
        {
            MessageBox.Show("Hello World");
        }

        public ViewModel()
        {
            Name = "Me Willie";
            SelectedIndex = -1;
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
