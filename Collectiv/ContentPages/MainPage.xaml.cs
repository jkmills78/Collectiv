using Collectiv.Interfaces;
using Collectiv.ViewModels;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;

namespace Collectiv.ContentPages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            BindingContext = mainPageViewModel;
        }
    }
}