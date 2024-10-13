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

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        ViewModel.ItemViewModel.LoadAttributes();
        ViewModel.ItemViewModel.LoadFilePackages();
    }

    protected override bool OnBackButtonPressed()
    {
        foreach (var filePackageViewModel in ViewModel.ItemViewModel.FilePackageViewModels)
        {
            Task.Run(filePackageViewModel.Cancel).Wait();
        }

        Task.Run(ViewModel.ItemViewModel.Cancel).Wait();

        return base.OnBackButtonPressed();
    }
}