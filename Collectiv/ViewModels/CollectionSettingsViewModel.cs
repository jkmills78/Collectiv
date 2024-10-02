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

        [RelayCommand]
        async Task DeleteCollection()
        {
            if (CollectionViewModel is null)
            {
                return;
            }

            throw new NotImplementedException();
        }
    }
}
