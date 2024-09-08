using Collectiv.Bases;
using Collectiv.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Collectiv.ViewModels
{
    public class CollectionViewModel : ObservableBase
    {
        public Action RefreshCollectionList { get; set; }

        private Action removeSelf;
        private Action<int> selectSelf;

        private bool isSettingsToggled;
        private bool isConfirmed;
        private readonly ApplicationDbContext dbContext;
        private Collection collection;
        private ICollection<ItemViewModel> itemViewModels;
        private ICollection<AttributeViewModel> availableAttributeViewModels;

        public bool IsSettingsToggled { get => isSettingsToggled; set { isSettingsToggled = value; OnPropertyChanged(); } }

        public bool IsConfirmed { get => isConfirmed; set { isConfirmed = value; OnPropertyChanged(); } }
        
        public Collection Collection { get => collection; set { collection = value; OnPropertyChanged(); } }
        
        public ICollection<ItemViewModel> ItemViewModels { get => itemViewModels; set => itemViewModels = value; }

        public ICollection<AttributeViewModel> AvailableAttributeViewModels { get => availableAttributeViewModels; set { availableAttributeViewModels = value; OnPropertyChanged(); } }

        //public void LoadCollection()
        //{
        //    LoadAvailableAttributes();
        //    LoadItems();
        //}

        //private void LoadAvailableAttributes()
        //{
        //    AvailableAttributeViewModels = new ObservableCollection<AttributeViewModel>();
        //    App.DbContext.CollectionAttribute.Load();
        //    foreach (var item in App.DbContext.Item.Local.Where(x => x.Collection.Id == collection.Id))
        //    {
        //        ItemViewModels.Add(new ItemViewModel
        //        {
        //            Item = item
        //        });
        //    }
        //}

        //private void LoadItems()
        //{
        //    ItemViewModels = new ObservableCollection<ItemViewModel>();
        //    App.DbContext.Item.Load();
        //    foreach (var item in App.DbContext.Item.Local.Where(x => x.Collection.Id == collection.Id))
        //    {
        //        ItemViewModels.Add(new ItemViewModel
        //        {
        //            Item = item
        //        });
        //    }
        //}

        public CollectionViewModel(Action removeSelf, Action<int> selectSelf)
        {
            this.removeSelf = removeSelf;
            this.selectSelf = selectSelf;
        }

        public ICommand ConfirmCommand => new RelayCommand(execute =>
        {
            App.DbContext.Collection.Add(Collection);
            IsConfirmed = true;
        });

        public ICommand CancelCommand => new RelayCommand(execute =>
        {
            selectSelf(collection.Id);
            if (App.DbContext.Collection.Any(collection => collection.Id == this.collection.Id))
            {
                // revert
                //collection = App.DbContext.Collection.SingleOrDefault(collection => collection.Id == this.collection.Id);
                foreach (var entry in App.DbContext.ChangeTracker.Entries())
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified; //Revert changes made to deleted entity.
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                    }
                }
                IsConfirmed = true;
                RefreshCollectionList();
            }
            else
            {
                removeSelf();
            }
        });

        public ICommand ToggleSettingsCommand => new RelayCommand(execute =>
        {
            selectSelf(collection.Id);
            IsSettingsToggled = !IsSettingsToggled;
        });

        public ICommand EditNameCommand => new RelayCommand(execute =>
        {
            IsConfirmed = !IsConfirmed;
        });
    }
}
