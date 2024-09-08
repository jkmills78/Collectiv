using CollectionTracker;
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
    public class MainWindowViewModel : IDisposable
    {
        public string Title { get; set; }
        public ICollection<Models.Collection> Collections { get; set; }

        private readonly ApplicationDbContext dbContext;
        private Models.Collection currentCollection;

        public MainWindowViewModel()
        {
            dbContext = new ApplicationDbContext();

            Collections = new ObservableCollection<Models.Collection>();

            // this is for demo purposes only, to make it easier
            // to get up and running
            dbContext.Database.EnsureCreated();
            // load the entities into EF Core
            dbContext.Collection.Load();

            Collections = dbContext.Collection.Local.ToObservableCollection();
        }

        public ICommand AddCollectionCommand => new RelayCommand(execute =>
        {
            Collections.Add(new Models.Collection(RemoveCollection)
            {
                Id = Collections.Any() ? Collections.Max(x => x.Id) + 1 : 1
            });
        });

        public ICommand RemoveCollectionCommand => new RelayCommand(execute =>
        {
            RemoveCollection(currentCollection.Id);
        });

        private void RemoveCollection()
        {
            Collections.Remove(currentCollection);
        }

        private void RemoveCollection(int id)
        {
            var collection = Collections.SingleOrDefault(collection => collection.Id == id);
            if (collection != null)
            {
                Collections.Remove(collection);
            }
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public void SetCurrentCollection(object currentCollection)
        {
            this.currentCollection = (Models.Collection)currentCollection;
        }
    }
}
