using Castle.Components.DictionaryAdapter;
using Collectiv.Abstracts;
using Collectiv.Common.DTOs;
using Collectiv.ContentPages;
using Collectiv.Interfaces;
using Collectiv.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Attribute = Collectiv.Models.Attribute;

namespace Collectiv.ViewModels
{
    public partial class CollectionViewModel : ContainerViewModel<CollectionViewModel>
    {
        [ObservableProperty]
        private ICollection<ItemViewModel> itemViewModels;

        public CollectionViewModel(IServiceProvider serviceProvider, Container container, Action<ContainerViewModel<CollectionViewModel>> cancel, Func<ContainerViewModel<CollectionViewModel>,Task> remove)
            : base(serviceProvider, container, cancel, remove)
        {
            ItemViewModels = new ObservableCollection<ItemViewModel>();

            AttributeViewModels.SelectedItemChanged += AttributeViewModels_SelectedItemChanged;
        }

        private void AttributeViewModels_SelectedItemChanged(object sender, AttributeViewModel e)
        {
            Guid attributeId = new Guid();
            {
                if (e is AttributeViewModel attributeViewModel)
                {
                    attributeId = attributeViewModel.Attribute.Id;
                }
            }

            if (attributeId != Guid.Empty)
            {
                foreach (var attributeViewModel in AttributeViewModels)
                {
                    if (attributeViewModel.Attribute.Id == attributeId)
                    {
                        attributeViewModel.IsSelected = true;
                    }
                    else
                    {
                        attributeViewModel.IsSelected = false;
                    }
                }
            }
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
            var itemContainer = new Container()
            {
                Id = Guid.NewGuid(), // Get the next Id and assign
                ParentId = Container.Id,
                Type = ContainerType.Item,
                Sequence = ItemViewModels.Select(viewModel => viewModel?.Container?.Sequence).Max() + 1 ?? 1
            };

            var itemViewModel = new ItemViewModel(serviceProvider, itemContainer, CancelItem, RemoveItem);

            foreach (var attributeViewModel in AttributeViewModels)
            {
                itemViewModel.AddAttribute(attributeViewModel);
            }

            ItemViewModels.Add(itemViewModel);
        }

        [RelayCommand]
        async Task AddAttribute()
        {
            var attributeViewModel = new AttributeViewModel(serviceProvider, CancelAttribute, AddAttributeToItems)
            {
                Attribute = new Attribute()
                {
                    Id = Guid.NewGuid(), // Get the next Id and assign
                    ContainerId = Container.Id
                }
            };

            AttributeViewModels.Add(attributeViewModel);
        }

        [RelayCommand]
        async Task RemoveAttribute()
        {
            if (AttributeViewModels.SelectedItem == null)
            {
                return;
            }

            await applicationDbService.RemoveAsync<Attribute>(AttributeViewModels.SelectedItem.Attribute.Id);
            AttributeViewModels.Remove(AttributeViewModels.SelectedItem);
        }

        #endregion

        #region Loaders

        public void LoadAttributes()
        {
            AttributeViewModels.Clear();

            if (Container != null)
            {
                foreach (var attribute in Container.Attributes)
                {
                    AttributeViewModels.Add(new AttributeViewModel(serviceProvider, CancelAttribute, AddAttributeToItems) { Attribute = attribute, IsConfirmed = true });
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
                    var itemViewModel = new ItemViewModel(serviceProvider, child, CancelItem, RemoveItem) { IsConfirmed = true };

                    Task.Run(itemViewModel.LoadCoverImage).Wait();
                    ItemViewModels.Add(itemViewModel);
                };
            }
        }

        #endregion

        public async Task RemoveItem(ContainerViewModel<ItemViewModel> itemViewModel)
        {
            if (App.HostMode.Value == "Hosted")
            {
                await restService.DeleteFilePackagesAsync(itemViewModel.Container.Id);
            }

            await applicationDbService.RemoveAsync<Container>(itemViewModel.Container.Id);
            ItemViewModels.Remove(itemViewModel as ItemViewModel);
        }

        private void CancelItem(ContainerViewModel<ItemViewModel> itemViewModel)
        {
            ItemViewModels.Remove(itemViewModel as ItemViewModel);
        }

        private void CancelAttribute(AttributeViewModel attributeViewModel)
        {
            AttributeViewModels.Remove(attributeViewModel);
        }

        private async Task AddAttributeToItems(AttributeViewModel attributeViewModel)
        {
            foreach (var itemViewModel in ItemViewModels)
            {
                var attribute = new Attribute
                {
                    Id = Guid.NewGuid(),
                    ContainerId = itemViewModel.Container.Id,
                    Name = attributeViewModel.Attribute.Name
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
