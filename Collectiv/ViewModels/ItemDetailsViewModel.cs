using Collectiv.ContentPages;
using Collectiv.Interfaces;
using Collectiv.Models;
using Collectiv.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.ViewModels

{
    [QueryProperty("ItemViewModel", "ItemViewModel")]
    [QueryProperty("FilePackageViewModel", "FilePackageViewModel")]
    public partial class ItemDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ItemViewModel itemViewModel;

        [ObservableProperty]
        private FilePackageViewModel filePackageViewModel;

        private ApplicationDbService applicationDbService;
        private SettingsDbService settingsDbService;
        private IRESTService restService;


        public ItemDetailsViewModel(IServiceProvider serviceProvider)
        {
            applicationDbService = serviceProvider.GetService<ApplicationDbService>();
            settingsDbService = serviceProvider.GetService<SettingsDbService>();
            restService = serviceProvider.GetRequiredService<IRESTService>();
            PropertyChanged += ItemDetailsViewModel_PropertyChanged;
        }

        private async void ItemDetailsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FilePackageViewModel))
            {
                if (FilePackageViewModel is not null)
                {
                    // Delete
                    if (FilePackageViewModel.IsMarkedForDeletion)
                    {
                        if (App.HostMode.Value == "Hosted")
                        {
                            await restService.DeleteFilePackageAsync(FilePackageViewModel.FilePackage.ContainerId, FilePackageViewModel.FilePackage.Id);
                        }

                        await applicationDbService.RemoveAsync<FilePackage>(FilePackageViewModel.FilePackage.Id);

                        ItemViewModel.FilePackageViewModels.Remove(FilePackageViewModel);
                        var filePackage = ItemViewModel.Container.FilePackages.SingleOrDefault(filePackage => filePackage.Id == FilePackageViewModel.FilePackage.Id);
                        if ((filePackage != null))
                        {
                            ItemViewModel.Container.FilePackages.Remove(ItemViewModel.Container.FilePackages.Single(filePackage => filePackage.Id == FilePackageViewModel.FilePackage.Id));
                        }
                        return;
                    }
                }
            }
        }

        [RelayCommand]
        async Task GoToSettings()
        {
            if (ItemViewModel is null)
            {
                return;
            }

            await Shell.Current.GoToAsync(nameof(ItemSettings), true, new Dictionary<string, object>
            {
                { "ItemViewModel", ItemViewModel }
            });
        }
    }
}