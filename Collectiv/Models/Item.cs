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

namespace Collectiv.Models
{
    public class Item : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string description;
        private ICollection<Attribute> attributes;
        private ICollection<File> files;
        private Collection collection;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get => id; set { id = value; OnPropertyChanged(); } }

        public string Name { get => name; set { name = value; OnPropertyChanged(); } }

        public string Description { get => description; set { description = value; OnPropertyChanged(); } }

        public virtual ICollection<Attribute> Attributes { get => attributes; set { attributes = value; OnPropertyChanged(); } }

        public virtual ICollection<File> Files { get => files; set { files = value; OnPropertyChanged(); } }

        public virtual Collection Collection { get => collection; set { collection = value; OnPropertyChanged(); } }


        public Item()
        {
            Attributes = new ObservableCollection<Attribute>();
            Files = new ObservableCollection<File>();
        }

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
