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

        public string Name { get; set; }
        public ObservableCollection<Item> Items { get; set; }

        AddDialogViewModel dialog = new AddDialogViewModel();


        private ICommand _showCommand;
        private ICommand _deleteCommand;
        private ICommand _showOneCommand;
        private ICommand _helloWorldCOmmand;
        private ICommand _addCommand;
        public ICommand ShowCommand => _showCommand ?? (_showCommand = new Command(Show, CanShow));
        public ICommand ShowOneCommand => _showOneCommand ?? (_showOneCommand = new Command(ShowOne));
        public ICommand DeleteCommand
        {
            get
            {
                if ( _deleteCommand == null )
                {
                    _deleteCommand = new Command(Delete, CanShow);
                    _deleteCommand.CanExecuteChanged += _deleteCommand_CanExecuteChanged;
                }
                return _deleteCommand; 
            }
        }

        private void _deleteCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

        }

        public ICommand AddCommand => _addCommand ?? (_addCommand = new Command(Insert));
        private ICommand _EditCommand;
        public ICommand EditCommand => _EditCommand ?? (_EditCommand = new Command(Edit, IsOneSelected));


        public ICommand HelloWorldCommand => _helloWorldCOmmand ?? (_helloWorldCOmmand = new Command(HelloWorld));

        public int SelectedIndex { get; set; }

        private List<Item> objectToList(object param)
        {
            if (param == null) return null;
            
            //else:
            System.Collections.IList list = (System.Collections.IList)param;
            var collection = list.Cast<Item>();
            return collection.ToList<Item>();
        }

        public bool IsOneSelected(object param)
        {
            if (param == null) return false;

            var selected = objectToList(param);
            if (selected.Count == 1) return true;
            else return false;
        }
        public void Edit(object param)
        {
            List<Item> selected = objectToList(param);
            selected[0].Name = "edited";
        }

        public void Show(object param)
        {
            var items = objectToList(param);

            string output = "Selected: \n";
            foreach (Item item in items)
            {
                output += item.ToString() + "\n"; 
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
            var collection = items.Cast<Item>();

            if (items.Count == 0) return false;
            else return true;
        }
        
        public void Delete(object param)
        {
            var items = objectToList(param);

            foreach (Item item in items)
            {
                Items.Remove(item);
            }
            
        }
        public void Insert(object param)
        {
            string name = dialog.ShowDialog();
            if (name == "") name = "untitled";

            int i = (int)param;
            if (i == -1)
            {
                Items.Add(new Item(name));
                SelectedIndex = Items.Count - 1;
            }
            else
            {
                Items.Insert(i + 1, new Item(name));
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
            Items = new ObservableCollection<Item>
            {
                new Item("Klaymen"),
                new Item("Hoborg"),
                new Item("Klogg"),
                new Item("Willie Trombone")
            };
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
