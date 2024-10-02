using Collectiv.ViewModels;

namespace Collectiv.ContentPages;

public partial class ItemDetails : ContentPage
{
    public ItemDetailsViewModel ViewModel { get => (ItemDetailsViewModel)BindingContext; set { BindingContext = value; } }
    public ItemDetails(ItemDetailsViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        ViewModel.ItemViewModel.LoadAttributes();
        await ViewModel.ItemViewModel.LoadFilePackages();
    }
}