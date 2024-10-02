using Collectiv.Interfaces;
using Collectiv.Models;
using Collectiv.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Collectiv.Bases
{
    public abstract partial class ViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isConfirmed;

        private bool isBusy;

        internal ApplicationDbService applicationDbService;
        internal readonly SettingsDbService settingsDbService;
        internal IRESTService restService;
        internal IFileService fileService;
        internal IServiceProvider serviceProvider;

        public ViewModel(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            applicationDbService = serviceProvider.GetService<ApplicationDbService>();
            settingsDbService = serviceProvider.GetService<SettingsDbService>();
            restService = serviceProvider.GetService<IRESTService>();
            fileService = serviceProvider.GetService<IFileService>();
        }

        internal async Task ExecuteCommand(Func<Task> command)
        {
            if (isBusy)
            {
                return;
            }

            try
            {
                isBusy = true;
                await command.Invoke();
            }
            catch
            {
                await Shell.Current.DisplayAlert("Error", "An error occurred!", "Ok");
            }
            finally
            {
                isBusy = false;
            }
        }
    }
}
