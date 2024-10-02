using Collectiv.Bases;
using Collectiv.ContentPages;
using Collectiv.Interfaces;
using Collectiv.Models;
using System.Collections.ObjectModel;
using Attribute = Collectiv.Models.Attribute;

namespace Collectiv.ViewModels
{
    public partial class ItemViewModel : ViewModel
    {
        [ObservableProperty]
        private FileViewModel coverImage;

        [ObservableProperty]
        private ICollection<AttributeViewModel> attributeViewModels;

        [ObservableProperty]
        private ICollection<FilePackageViewModel> filePackageViewModels;

        #region Backing Model

        [ObservableProperty]
        private Container container;

        #endregion

        private Action<ItemViewModel> cancel;

        private IRESTService restService;
        private IFileService fileService;

        private string previousName;

        public ItemViewModel(IServiceProvider serviceProvider, Action<ItemViewModel> cancel)
            : base(serviceProvider)
        {
            AttributeViewModels = new ObservableCollection<AttributeViewModel>();
            FilePackageViewModels = new ObservableCollection<FilePackageViewModel>();
            restService = serviceProvider.GetService<IRESTService>();
            this.cancel = cancel;
        }

        [RelayCommand]
        async Task GoToItemDetails()
        {
            if (Container is null)
            {
                return;
            }

            await Shell.Current.GoToAsync(nameof(ItemDetails), true, new Dictionary<string, object>
            {
                { "ItemViewModel", this }
            });
        }

        [RelayCommand]
        async Task AddFilePackage()
        {
            var filePackage = new FilePackage()
            {
                Id = Guid.NewGuid(), // Get the next Id and assign
                ContainerId = Container.Id,
                Container = Container
            };

            var filePackageViewModel = new FilePackageViewModel(serviceProvider, filePackage);

            await Shell.Current.GoToAsync(nameof(FilePackageDetails), true, new Dictionary<string, object>
            {
                { "FilePackageViewModel", filePackageViewModel }
            });
        }

        [RelayCommand]
        async Task RemoveFilePackage()//(FilePackageViewModel filePackageViewModel)
        {
            //await applicationDbService.DeleteAsync<FilePackage>(filePackageViewModel.FilePackage.Id);
            //FilePackageViewModels.Remove(filePackageViewModel);
            //await Shell.Current.GoToAsync(nameof(CollectionDetails), true, new Dictionary<string, object>
            //{
            //    { "CollectionViewModel", this }
            //});
        }

        [RelayCommand]
        async Task EditFilePackage(FilePackageViewModel filePackageViewModel)
        {
            await Shell.Current.GoToAsync(nameof(FilePackageDetails), true, new Dictionary<string, object>
            {
                { "FilePackageViewModel", filePackageViewModel }
            });
        }

        [RelayCommand]
        async Task ConfirmContainer()
        {
            if (await applicationDbService.ExistsAsync<Container>(Container.Id))
            {
                await applicationDbService.UpdateAsync(Container);
            }
            else
            {
                await applicationDbService.AddAsync(Container);
            }
            IsConfirmed = true;
        }

        [RelayCommand]
        async Task CancelContainer()
        {
            if (await applicationDbService.ExistsAsync<Container>(Container.Id))
            {
                Container.Name = previousName;
            }
            else
            {
                cancel(this);
            }
            IsConfirmed = true;
        }

        [RelayCommand]
        void EditContainerName()
        {
            previousName = Container.Name;
            IsConfirmed = !IsConfirmed;
        }

        public void LoadAttributes()
        {
            AttributeViewModels.Clear();

            if (Container != null)
            {
                foreach (var attribute in Container.Attributes)
                {
                    AttributeViewModels.Add(new AttributeViewModel(serviceProvider, CancelAttribute, null) { Attribute = attribute, IsConfirmed = true });
                };
            }
        }

        public async Task LoadFilePackages()
        {
            FilePackageViewModels.Clear();

            if (Container != null)
            {
                foreach (var filePackage in Container.FilePackages)
                {
                    var filePackageViewModel = new FilePackageViewModel(serviceProvider, filePackage) { IsConfirmed = true };
                    foreach (var file in filePackage.Files)
                    {
                        byte[] fileData;
                        if (App.HostMode.Value == "Hosted")
                        {
                            fileData = await restService.GetFileAsync(filePackage.ContainerId, filePackage.Id, Path.GetFileName(file.FullPath));
                        }
                        else
                        {
                            fileData = System.IO.File.ReadAllBytes(file.FullPath);
                        }

                        var fileViewModel = new FileViewModel(serviceProvider, file) { FileData = fileData };
                        filePackageViewModel.FileViewModels.Add(fileViewModel);

                        if (filePackage.IsPrimary && file.IsPrimary)
                        {
                            CoverImage = fileViewModel;
                        }
                    }
                    FilePackageViewModels.Add(filePackageViewModel);
                };
            }
        }

        public async void AddAvailableAttribute(AttributeViewModel collectionAttributeViewModel)
        {
            var itemAttributeViewModel = new AttributeViewModel(serviceProvider, CancelAttribute, null)
            {
                Attribute = new Attribute()
                {
                    Id = Guid.NewGuid(),
                    Name = collectionAttributeViewModel.Attribute.Name
                }
            };

            AttributeViewModels.Add(itemAttributeViewModel);
            Container.Attributes.Add(itemAttributeViewModel.Attribute);
        }

        private void CancelAttribute(AttributeViewModel attributeViewModel)
        {
            AttributeViewModels.Remove(attributeViewModel);
        }
    }
}
