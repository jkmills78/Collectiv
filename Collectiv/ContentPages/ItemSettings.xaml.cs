using Collectiv.ViewModels;

namespace Collectiv.ContentPages;

public partial class ItemSettings : ContentPage
{
    public ItemSettingsViewModel ViewModel { get => (ItemSettingsViewModel)BindingContext; set { BindingContext = value; } }
    public ItemSettings(ItemSettingsViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
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