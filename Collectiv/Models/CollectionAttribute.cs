using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collectiv.Abstracts;

namespace Collectiv.Models
{
    public class CollectionAttribute : Abstracts.Attribute
    {
        private Collection collection;

        public virtual Collection Collection { get => collection; set { collection = value; OnPropertyChanged(); } }
    }
}
