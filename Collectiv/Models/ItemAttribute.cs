using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collectiv.Abstracts;

namespace Collectiv.Models
{
    public class ItemAttribute : Abstracts.Attribute
    {
        private Item item;

        public virtual Item Item { get => item; set { item = value; OnPropertyChanged(); } }
    }
}
