using Collectiv;
using Collectiv.Bases;
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
                CollectionViewModels.Add(new CollectionViewModel(serviceProvider, container, CancelCollection) { IsConfirmed = true });
            };
        }

        [RelayCommand]
        async Task AddCollection()
        {
            await ExecuteCommand(async () =>
            {
                var container = new Container()
                {
                    Id = Guid.NewGuid(), // Get the next Id and assign
                    Type = ContainerType.Collection
                };

                CollectionViewModels.Add(new CollectionViewModel(serviceProvider, container, CancelCollection));
            });
        }

        private void CancelCollection(CollectionViewModel collectionViewModel)
        {
            CollectionViewModels.Remove(collectionViewModel);
        }
    }
}
