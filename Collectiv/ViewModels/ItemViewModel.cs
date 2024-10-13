using Collectiv.Abstracts;
using Collectiv.ContentPages;
using Collectiv.Interfaces;
using Collectiv.Models;
using System.Collections.ObjectModel;
using Attribute = Collectiv.Models.Attribute;

namespace Collectiv.ViewModels
{
    public partial class ItemViewModel : ContainerViewModel<ItemViewModel>
    {
        public ItemViewModel(IServiceProvider serviceProvider, Container container, Action<ContainerViewModel<ItemViewModel>> cancel, Func<ContainerViewModel<ItemViewModel>, Task> remove)
            : base(serviceProvider, container, cancel, remove)
        {

        }

        #region Commands

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

        #endregion

        #region Loaders

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

        #endregion

        public async Task AddAttribute(AttributeViewModel collectionAttributeViewModel)
        {
            var attributeViewModel = new AttributeViewModel(serviceProvider, CancelAttribute, null)
            {
                Attribute = new Attribute()
                {
                    Id = Guid.NewGuid(),
                    Name = collectionAttributeViewModel.Attribute.Name,
                    Sequence = AttributeViewModels.Select(viewModel => viewModel?.Attribute?.Sequence).Max() + 1 ?? 1
                }
            };

            AttributeViewModels.Add(attributeViewModel);
            Container.Attributes.Add(attributeViewModel.Attribute);
        }

        private void CancelAttribute(AttributeViewModel attributeViewModel)
        {
            AttributeViewModels.Remove(attributeViewModel);
        }
    }
}
