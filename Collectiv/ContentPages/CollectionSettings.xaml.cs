using Collectiv.ViewModels;

namespace Collectiv.ContentPages;

public partial class CollectionSettings : ContentPage
{
    public CollectionSettingsViewModel ViewModel { get => (CollectionSettingsViewModel)BindingContext; set { BindingContext = value; } }
    public CollectionSettings(CollectionSettingsViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
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