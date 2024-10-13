using Collectiv.ContentPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.ViewModels
{
    [QueryProperty("ItemViewModel", "ItemViewModel")]
    public partial class ItemSettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ItemViewModel itemViewModel;

        public ItemSettingsViewModel()
        {
            
        }
    }
}
