using Collectiv.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.ViewModels
{
    public class AttributeViewModel : ObservableBase
    {
        private Attribute attribute;

        public Attribute Attribute { get => attribute; set { attribute = value; OnPropertyChanged(); } }
    }
}
