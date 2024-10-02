using Collectiv.ViewModels;

namespace Collectiv.ContentPages;

public partial class HostSettings : ContentPage
{
    public HostSettingsViewModel ViewModel { get => (HostSettingsViewModel)BindingContext; set { BindingContext = value; } }
    public HostSettings(HostSettingsViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
    }
}