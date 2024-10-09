using Collectiv.Common.DTOs;
using Collectiv.ContentPages;
using Collectiv.Interfaces;
using Collectiv.Models;
using Collectiv.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Collectiv.ViewModels
{
    [QueryProperty("CollectionViewModel", "CollectionViewModel")]
    [QueryProperty("FilePackageViewModel", "FilePackageViewModel")]
    public partial class CollectionDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        private CollectionViewModel collectionViewModel;

        [ObservableProperty]
        private FilePackageViewModel filePackageViewModel;

        private ApplicationDbService applicationDbService;
        private SettingsDbService settingsDbService;
        private IRESTService restService;


        public CollectionDetailsViewModel(IServiceProvider serviceProvider)
        {
            applicationDbService = serviceProvider.GetService<ApplicationDbService>();
            settingsDbService = serviceProvider.GetService<SettingsDbService>();
            restService = serviceProvider.GetRequiredService<IRESTService>();
            PropertyChanged += CollectionDetailsViewModel_PropertyChanged;
        }

        private async void CollectionDetailsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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

                        CollectionViewModel.FilePackageViewModels.Remove(FilePackageViewModel);
                        var filePackage = CollectionViewModel.Container.FilePackages.SingleOrDefault(filePackage => filePackage.Id == FilePackageViewModel.FilePackage.Id);
                        if ((filePackage != null))
                        {
                            CollectionViewModel.Container.FilePackages.Remove(CollectionViewModel.Container.FilePackages.Single(filePackage => filePackage.Id == FilePackageViewModel.FilePackage.Id));
                        }
                        return;
                    }
                }

            }
        }

        [RelayCommand]
        async Task GoToSettings()
        {
            if (CollectionViewModel is null)
            {
                return;
            }

            await Shell.Current.GoToAsync(nameof(CollectionSettings), true, new Dictionary<string, object>
            {
                { "CollectionViewModel", CollectionViewModel }
            });
        }
    }
}
