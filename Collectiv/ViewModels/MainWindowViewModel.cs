using Collectiv;
using Collectiv.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Collectiv.ViewModels
{
    public class MainWindowViewModel : ObservableBase
    {
        private CollectionViewModel currentCollectionViewModel;
        private ICollection<CollectionViewModel> collectionViewModels;
        private string title;

        public CollectionViewModel CurrentCollectionViewModel { get => currentCollectionViewModel; set { currentCollectionViewModel = value; OnPropertyChanged(); } }
        
        public ICollection<CollectionViewModel> CollectionViewModels { get => collectionViewModels; set => collectionViewModels = value; }

        public string Title { get => title; set { title = value; } }

        public MainWindowViewModel()
        {
            CurrentCollectionViewModel = new CollectionViewModel(RemoveCollection, SelectCollection);
            CollectionViewModels = new ObservableCollection<CollectionViewModel>();

            //App.DbContext.Database.EnsureDeleted();
            // this is for demo purposes only, to make it easier
            // to get up and running
            App.DbContext.Database.EnsureCreated();
            // load the entities into EF Core
            App.DbContext.Collection.Load();

            foreach (var collection in App.DbContext.Collection.Local)
            {
                CollectionViewModels.Add(new CollectionViewModel(RemoveCollection, SelectCollection) { Collection = collection, IsConfirmed = true });
            };
        }

        public ICommand AddCollectionCommand => new RelayCommand(execute =>
        {
            CollectionViewModels.Add(new CollectionViewModel(RemoveCollection, SelectCollection)
            {
                Collection = new Models.Collection()
                {
                    Id = CollectionViewModels.Any() ? CollectionViewModels.Max(x => x.Collection.Id) + 1 : 1 // Get the next Id and assign to the new collection
                }
            });
        });

        public ICommand RemoveCollectionCommand => new RelayCommand(execute =>
        {
            RemoveCollection();
        });

        public ICommand SaveCommand => new RelayCommand(execute =>
        {
            App.DbContext.UpdateRange(CollectionViewModels.Select(x => x.Collection));
            // save data
            App.DbContext.SaveChanges();
        });

        private void RemoveCollection()
        {
            CollectionViewModels.Remove(currentCollectionViewModel);
        }

        //private void RemoveCollection(int id)
        //{
        //    var collectionViewModel = CollectionViewModels.SingleOrDefault(collectionViewModel => collectionViewModel.Collection.Id == id);
        //    if (collectionViewModel != null)
        //    {
        //        CollectionViewModels.Remove(collectionViewModel);
        //    }
        //}

        private void SelectCollection(int id)
        {
            CurrentCollectionViewModel = CollectionViewModels.SingleOrDefault(viewModel => viewModel.Collection.Id == id);
        }

        public void SetCurrentCollection(object currentCollectionViewModel)
        {
            this.currentCollectionViewModel = (CollectionViewModel)currentCollectionViewModel;
        }
    }
}
