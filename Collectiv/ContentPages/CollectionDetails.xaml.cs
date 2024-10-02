using Collectiv.ViewModels;

namespace Collectiv.ContentPages;

public partial class CollectionDetails : ContentPage
{
    public CollectionDetailsViewModel ViewModel { get => (CollectionDetailsViewModel)BindingContext; set { BindingContext = value; } }
	public CollectionDetails(CollectionDetailsViewModel viewModel)
	{
		InitializeComponent();
        ViewModel = viewModel;
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        ViewModel.CollectionViewModel.LoadChildren();
        ViewModel.CollectionViewModel.LoadAvailableAttributes();
        await ViewModel.CollectionViewModel.LoadFilePackages();
    }
}