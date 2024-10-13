using Collectiv.Abstracts;
using Attribute = Collectiv.Models.Attribute;

namespace Collectiv.ViewModels
{
    public partial class AttributeViewModel : ViewModel
    {
        [ObservableProperty]
        private bool isSelected;

        [ObservableProperty]
        private bool isConfirmed;

        [ObservableProperty]
        private Attribute attribute;

        private Action<AttributeViewModel> cancel;
        private Func<AttributeViewModel,Task> addAttributeToItems;

        private string oldName;
        private string oldValue;

        public AttributeViewModel(IServiceProvider serviceProvider, Action<AttributeViewModel> cancel, Func<AttributeViewModel,Task> addAttributeToItems)
            :base(serviceProvider)
        {
            this.cancel = cancel;
            this.addAttributeToItems = addAttributeToItems;
        }

        [RelayCommand]
        async Task ConfirmAttribute()
        {
            if (await applicationDbService.ExistsAsync<Attribute>(Attribute.Id))
            {
                await applicationDbService.UpdateAsync(Attribute);
            }
            else
            {
                await applicationDbService.AddAsync(Attribute);
            }
            if (addAttributeToItems != null)
            {
                await addAttributeToItems.Invoke(this);
            }
            IsConfirmed = true;
        }

        [RelayCommand]
        async Task CancelAttribute()
        {
            if (await applicationDbService.ExistsAsync<Attribute>(Attribute.Id))
            {
                Attribute.Name = oldName;
                Attribute.Value = oldValue;
            }
            else
            {
                cancel(this);
            }
            IsConfirmed = true;
        }

        [RelayCommand]
        async Task EditAttribute()
        {
            oldName = Attribute.Name;
            oldValue = Attribute.Value;
            IsConfirmed = !IsConfirmed;
        }
    }
}
