using Collectiv.Bases;
using Collectiv.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.ViewModels
{
    public class ItemViewModel : ObservableBase
    {
        private Item item;

        public Item Item { get => item; set { item = value; OnPropertyChanged(); } }
    }
}
