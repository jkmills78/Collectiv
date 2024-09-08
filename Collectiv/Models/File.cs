using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.Models
{
    public class File : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string description;
        private string path;
        private Item item;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get => id; set { id = value; OnPropertyChanged(); } }

        public string Name { get => name; set { name = value; OnPropertyChanged(); } }

        public string Description { get => description; set { description = value; OnPropertyChanged(); } }

        public string Path { get => path; set { path = value; OnPropertyChanged(); } }

        public virtual Item Item { get => item; set { item = value; OnPropertyChanged(); } }

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
