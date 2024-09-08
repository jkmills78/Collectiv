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
using Collectiv.Abstracts;
using Collectiv.Bases;

namespace Collectiv.Models
{
    public class Item : ObservableBase
    {
        private int id;
        private string name;
        private string description;
        private ICollection<ItemAttribute> attributes;
        private ICollection<File> files;
        private Collection collection;

        public int Id { get => id; set { id = value; OnPropertyChanged(); } }

        public string Name { get => name; set { name = value; OnPropertyChanged(); } }

        public string Description { get => description; set { description = value; OnPropertyChanged(); } }

        public virtual ICollection<ItemAttribute> Attributes { get => attributes; set { attributes = value; OnPropertyChanged(); } }

        public virtual ICollection<File> Files { get => files; set { files = value; OnPropertyChanged(); } }

        public virtual Collection Collection { get => collection; set { collection = value; OnPropertyChanged(); } }


        public Item()
        {
            Attributes = new ObservableCollection<ItemAttribute>();
            Files = new ObservableCollection<File>();
        }
    }
}
