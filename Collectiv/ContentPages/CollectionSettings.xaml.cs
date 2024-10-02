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
}