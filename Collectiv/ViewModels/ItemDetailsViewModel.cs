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
                        ItemViewModel.Container.FilePackages.Remove(ItemViewModel.Container.FilePackages.Single(filePackage => filePackage.Id == FilePackageViewModel.FilePackage.Id));
                        return;
                    }

                    if (FilePackageViewModel.IsConfirmed)
                    {
                        // New data must be posted no matter what
                        if (App.HostMode.Value == "Hosted")
                        {
                            var filePackageDto = new FilePackageDTO
                            {
                                ContainerId = FilePackageViewModel.FilePackage.ContainerId,
                                Id = FilePackageViewModel.FilePackage.Id,
                                Name = FilePackageViewModel.FilePackage.Name,
                                Description = FilePackageViewModel.FilePackage.Description
                            };

                            foreach (var fileViewModel in FilePackageViewModel.FileViewModels)
                            {
                                filePackageDto.Files.Add(new FileDTO
                                {
                                    FilePackageId = fileViewModel.File.FilePackageId,
                                    FileName = fileViewModel.FileName,
                                    FullPath = fileViewModel.File.FullPath,
                                    MimeType = fileViewModel.File.MimeType,
                                    FileData = fileViewModel.FileData
                                });
                            }

                            var response = await restService.PostFilePackageAsync(filePackageDto);
                        }

                        // Update
                        if (ItemViewModel.FilePackageViewModels.Any(filePackageViewModel => filePackageViewModel.FilePackage.Id == FilePackageViewModel.FilePackage.Id))
                        {
                            await applicationDbService.UpdateAsync(FilePackageViewModel.FilePackage);
                            var filePackageViewModel = ItemViewModel.FilePackageViewModels.SingleOrDefault(filePackageViewModel => filePackageViewModel.FilePackage.Id == FilePackageViewModel.FilePackage.Id);
                            if (filePackageViewModel is not null)
                            {
                                filePackageViewModel.FilePackage = FilePackageViewModel.FilePackage;
                            }
                        }

                        // Add
                        else
                        {
                            FilePackageViewModel.FilePackage.Container = null;
                            await applicationDbService.AddAsync(FilePackageViewModel.FilePackage);
                            ItemViewModel.FilePackageViewModels.Add(FilePackageViewModel);
                            ItemViewModel.Container.FilePackages.Add(FilePackageViewModel.FilePackage);
                        }
                    }
                }
            }
        }
    }
}