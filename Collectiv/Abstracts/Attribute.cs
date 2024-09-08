using Collectiv.Bases;
using Collectiv.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.Abstracts
{
    public abstract class Attribute : ObservableBase
    {
        private int id;
        private string name;
        private string value;
        private Item item;
        private Collection collection;

        public int Id { get => id; set { id = value; OnPropertyChanged(); } }

        public string Name { get => name; set { name = value; OnPropertyChanged(); } }

        public string Value { get => value; set { this.value = value; OnPropertyChanged(); } }
    }
}
