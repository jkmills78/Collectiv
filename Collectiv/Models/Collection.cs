using Collectiv.Abstracts;
using Collectiv.Bases;
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
    public class Collection : ObservableBase
    {
        private int id;
        private int sequence;
        private string name;
        private string description;
        private ICollection<CollectionAttribute> attributes;

        public int Id { get => id; set { id = value; OnPropertyChanged(); } }

        public int Sequence { get => sequence; set { sequence = value; OnPropertyChanged(); } }

        public string Name { get => name; set { name = value; OnPropertyChanged(); } }

        public string Description { get => description; set { description = value; OnPropertyChanged(); } }

        public virtual ICollection<CollectionAttribute> Attributes { get => attributes; set { attributes = value; OnPropertyChanged(); } }

        public Collection()
        {
            Attributes = new ObservableCollection<CollectionAttribute>();
        }
    }
}
