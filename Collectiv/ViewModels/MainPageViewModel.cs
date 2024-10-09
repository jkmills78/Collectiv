using Collectiv;
using Collectiv.Abstracts;
using Collectiv.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Collectiv.ContentPages;
using Collectiv.Services;
using Collectiv.Models;

namespace Collectiv.ViewModels
{
    public partial class MainPageViewModel : ViewModel
    {
        [ObservableProperty]
        private ICollection<CollectionViewModel> collectionViewModels;

        [ObservableProperty]
        private string title;

        public MainPageViewModel(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            CollectionViewModels = new ObservableCollection<CollectionViewModel>();

            foreach (var container in applicationDbService.GetAsync<Container>(ContainerType.Collection).Result)
            {
                var collectionViewModel = new CollectionViewModel(serviceProvider, container, CancelCollection, RemoveCollection) { IsConfirmed = true };

                Task.Run(collectionViewModel.LoadCoverImage).Wait();
                CollectionViewModels.Add(collectionViewModel);
            };
        }

        #region Commands

        [RelayCommand]
        async Task AddCollection()
        {
            await ExecuteCommand(async () =>
            {
                var container = new Container()
                {
                    Id = Guid.NewGuid(), // Get the next Id and assign
                    Type = ContainerType.Collection,
                    Sequence = CollectionViewModels.Select(viewModel => viewModel?.Container?.Sequence).Max() + 1 ?? 1
                };

                var collectionViewModel = new CollectionViewModel(serviceProvider, container, CancelCollection, RemoveCollection);
                await collectionViewModel.LoadCoverImage();
                CollectionViewModels.Add(collectionViewModel);
            });
        }

        #endregion

        private void CancelCollection(ContainerViewModel<CollectionViewModel> collectionViewModel)
        {
            CollectionViewModels.Remove(collectionViewModel as CollectionViewModel);
        }

        private async Task RemoveCollection(ContainerViewModel<CollectionViewModel> collectionViewModel)
        {
            if (App.HostMode.Value == "Hosted")
            {
                foreach (var itemViewModel in (collectionViewModel as CollectionViewModel)?.ItemViewModels)
                {
                    await (collectionViewModel as CollectionViewModel).RemoveItem(itemViewModel);
                }

                await restService.DeleteFilePackagesAsync(collectionViewModel.Container.Id);
            }

            await applicationDbService.RemoveAsync<Container>(collectionViewModel.Container.Id);
            CollectionViewModels.Remove(collectionViewModel as CollectionViewModel);
        }
    }
}
