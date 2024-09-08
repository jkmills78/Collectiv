using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Collectiv.Models
{
    public class Collection : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Action<int> removeChild;
        private int id;
        private int sequence;
        private bool isConfirmed;
        private string name;
        private string description;
        private ICollection<Item> items;

        public int Id { get => id; set { id = value; OnPropertyChanged(); } }

        public int Sequence { get => sequence; set { sequence = value; OnPropertyChanged(); } }

        public bool IsConfirmed { get => isConfirmed; set { isConfirmed = value; OnPropertyChanged(); } }

        public string Name { get => name; set { name = value; OnPropertyChanged(); } }

        public string Description { get => description; set { description = value; OnPropertyChanged(); } }

        public virtual ICollection<Item> Items { get => items; set { items = value; OnPropertyChanged(); } }

        public Collection()
        {
            Items = new ObservableCollection<Item>();
        }

        public Collection(Action<int> removeChild)
            : this()
        {
            this.removeChild = removeChild;
        }

        public ICommand ConfirmCommand => new RelayCommand(execute =>
        {
            IsConfirmed = true;
        });

        public ICommand CancelCommand => new RelayCommand(execute =>
        {
            removeChild(Id);
        });

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
