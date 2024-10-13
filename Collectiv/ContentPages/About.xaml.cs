using Collectiv.ViewModels;

namespace Collectiv.ContentPages;

public partial class About : ContentPage
{
    public AboutViewModel ViewModel { get => (AboutViewModel)BindingContext; set { BindingContext = value; } }
    public About(AboutViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
    }
}