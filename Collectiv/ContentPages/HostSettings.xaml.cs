using Collectiv.ViewModels;
using CommunityToolkit.Maui.Animations;
using CommunityToolkit.Maui.Behaviors;

namespace Collectiv.ContentPages;

public partial class HostSettings : ContentPage
{
    public HostSettingsViewModel ViewModel { get => (HostSettingsViewModel)BindingContext; set { BindingContext = value; } }
    public HostSettings(HostSettingsViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;

    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        Status.Text = "Host Settings Saved";
        Status.Opacity = 1;
        Status.FadeTo(0.0, 5000);
    }

    private void RevertButton_Clicked(object sender, EventArgs e)
    {
        Status.Text = "Host Settings Reverted";
        Status.Opacity = 1;
        Status.FadeTo(1, 0);
        Status.FadeTo(0, 5000);
    }
}