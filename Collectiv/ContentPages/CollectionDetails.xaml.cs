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

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        ViewModel.CollectionViewModel.LoadChildren();
        ViewModel.CollectionViewModel.LoadAttributes();
        ViewModel.CollectionViewModel.LoadFilePackages();
    }

    protected override bool OnBackButtonPressed()
    {
        foreach (var itemViewModel in ViewModel.CollectionViewModel.ItemViewModels)
        {
            Task.Run(itemViewModel.Cancel).Wait();
        }

        foreach (var filePackageViewModel in ViewModel.CollectionViewModel.FilePackageViewModels)
        {
            Task.Run(filePackageViewModel.Cancel).Wait();
        }

        Task.Run(ViewModel.CollectionViewModel.Cancel).Wait();

        return base.OnBackButtonPressed();
    }
}