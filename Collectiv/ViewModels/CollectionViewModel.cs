using Castle.Components.DictionaryAdapter;
using Collectiv.Bases;
using Collectiv.ContentPages;
using Collectiv.Interfaces;
using Collectiv.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Attribute = Collectiv.Models.Attribute;

namespace Collectiv.ViewModels
{
    public partial class CollectionViewModel : ViewModel
    {
        [ObservableProperty]
        private FileViewModel coverImage;

        [ObservableProperty]
        private ICollection<ItemViewModel> itemViewModels;

        [ObservableProperty]
        private ICollection<AttributeViewModel> availableAttributeViewModels;

        [ObservableProperty]
        private ICollection<FilePackageViewModel> filePackageViewModels;

        [ObservableProperty]
        private bool isHostLoading;

        private string previousName;

        #region Backing Model

        [ObservableProperty]
        private Container container;

        #endregion

        private Action<CollectionViewModel> cancel;

        public CollectionViewModel(IServiceProvider serviceProvider, Container container, Action<CollectionViewModel> cancel)
            : base(serviceProvider)
        {
            AvailableAttributeViewModels = new ObservableCollection<AttributeViewModel>();
            ItemViewModels = new ObservableCollection<ItemViewModel>();
            filePackageViewModels = new ObservableCollection<FilePackageViewModel>();
            Container = container;
            this.cancel = cancel;
        }

        #region Commands

        [RelayCommand]
        async Task GoToCollectionDetails()
        {
            if (Container is null)
            {
                return;
            }

            await Shell.Current.GoToAsync(nameof(CollectionDetails), true, new Dictionary<string, object>
            {
                { "CollectionViewModel", this }
            });
        }

        [RelayCommand]
        async Task AddItem()
        {
            var itemViewModel = new ItemViewModel(serviceProvider, CancelItem)
            {
                Container = new Container()
                {
                    Id = Guid.NewGuid(), // Get the next Id and assign
                    ParentId = Container.Id,
                    Type = ContainerType.Item
                }
            };

            foreach (var availableAttributeViewModel in AvailableAttributeViewModels)
            {
                itemViewModel.AddAvailableAttribute(availableAttributeViewModel);
            }

            ItemViewModels.Add(itemViewModel);
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

        [RelayCommand]
        async Task AddAttribute()
        {
            var availableAttributeViewModel = new AttributeViewModel(serviceProvider, CancelAttribute, AddAvailableAttributeToItems)
            {
                Attribute = new Attribute()
                {
                    Id = Guid.NewGuid(), // Get the next Id and assign
                    ContainerId = Container.Id
                }
            };

            AvailableAttributeViewModels.Add(availableAttributeViewModel);
        }

        #endregion

        #region Loaders

        public void LoadAvailableAttributes()
        {
            AvailableAttributeViewModels.Clear();

            if (Container != null)
            {
                foreach (var attribute in Container.Attributes)
                {
                    AvailableAttributeViewModels.Add(new AttributeViewModel(serviceProvider, CancelAttribute, AddAvailableAttributeToItems) { Attribute = attribute, IsConfirmed = true });
                };
            }
        }

        public void LoadChildren()
        {
            ItemViewModels.Clear();

            if (Container != null)
            {
                foreach (var child in Container.Children)
                {
                    ItemViewModels.Add(new ItemViewModel(serviceProvider, CancelItem) { Container = child, IsConfirmed = true });
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
                    foreach(var file in filePackage.Files)
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

                        if(filePackage.IsPrimary && file.IsPrimary)
                        {
                            CoverImage = fileViewModel;
                        }
                    }
                    FilePackageViewModels.Add(filePackageViewModel);
                };
            }
        }

        #endregion

        private void CancelItem(ItemViewModel itemViewModel)
        {
            ItemViewModels.Remove(itemViewModel);
        }

        private void CancelAttribute(AttributeViewModel attributeViewModel)
        {
            AvailableAttributeViewModels.Remove(attributeViewModel);
        }


        private async Task AddAvailableAttributeToItems(AttributeViewModel availableAttributeViewModel)
        {
            foreach (var itemViewModel in ItemViewModels)
            {
                var attribute = new Attribute
                {
                    Id = Guid.NewGuid(),
                    ContainerId = itemViewModel.Container.Id,
                    Name = availableAttributeViewModel.Attribute.Name
                };

                itemViewModel.AttributeViewModels.Add
                (
                    new AttributeViewModel(serviceProvider, null, null)
                    {
                        Attribute = attribute,
                        IsConfirmed = true
                    }
                );

                if(await applicationDbService.ExistsAsync<Attribute>(attribute.Id))
                {
                    await applicationDbService.UpdateAsync(attribute);
                }
                else
                {
                    await applicationDbService.AddAsync(attribute);
                }
            }
        }
    }
}
