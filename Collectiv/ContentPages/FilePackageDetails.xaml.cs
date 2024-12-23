using Collectiv.ViewModels;

namespace Collectiv.ContentPages;

public partial class FilePackageDetails : ContentPage
{
    public FilePackageDetailsViewModel ViewModel { get => (FilePackageDetailsViewModel)BindingContext; set { BindingContext = value; } }
    public FilePackageDetails(FilePackageDetailsViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
    }

    protected override bool OnBackButtonPressed()
    {
        Task.Run(ViewModel.FilePackageViewModel.Cancel).Wait();

        return base.OnBackButtonPressed();
    }
}