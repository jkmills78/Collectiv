using Collectiv.Bases;
using Attribute = Collectiv.Models.Attribute;

namespace Collectiv.ViewModels
{
    public partial class AttributeViewModel : ViewModel
    {
        [ObservableProperty]
        private bool isConfirmed;

        [ObservableProperty]
        private Attribute attribute;

        private Action<AttributeViewModel> cancel;
        private Func<AttributeViewModel,Task> addAvailableAttributeToItems;

        private string previousName;
        private string previousValue;

        public AttributeViewModel(IServiceProvider serviceProvider, Action<AttributeViewModel> cancel, Func<AttributeViewModel,Task> addAvailableAttributeToItems)
            :base(serviceProvider)
        {
            this.cancel = cancel;
            this.addAvailableAttributeToItems = addAvailableAttributeToItems;
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
            await addAvailableAttributeToItems(this);
            IsConfirmed = true;
        }

        [RelayCommand]
        async Task CancelAttribute()
        {
            if (await applicationDbService.ExistsAsync<Attribute>(Attribute.Id))
            {
                Attribute.Name = previousName;
                Attribute.Value = previousValue;
            }
            else
            {
                cancel(this);
            }
            IsConfirmed = true;
        }

        [RelayCommand]
        void EditAttribute()
        {
            previousName = Attribute.Name;
            previousValue = Attribute.Value;
            IsConfirmed = !IsConfirmed;
        }
    }
}
