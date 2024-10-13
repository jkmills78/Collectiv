using Collectiv.ContentPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.ViewModels
{
    [QueryProperty("CollectionViewModel", "CollectionViewModel")]
    public partial class CollectionSettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private CollectionViewModel collectionViewModel;

        public CollectionSettingsViewModel()
        {
            
        }
    }
}
