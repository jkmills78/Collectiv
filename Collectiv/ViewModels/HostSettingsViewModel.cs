using Collectiv.Models;
using Collectiv.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.ViewModels
{
    public partial class HostSettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        string hostModeValue;

        [ObservableProperty]
        string hostAddressValue;

        [ObservableProperty]
        string hostAPIKeyValue;

        [ObservableProperty]
        string hostUsernameValue;

        [ObservableProperty]
        string hostPasswordValue;

        private SettingsDbService settingsDbService;

        public HostSettingsViewModel(IServiceProvider serviceProvider)
        {
            HostModeValue = App.HostMode.Value;
            HostAddressValue = App.HostAddress.Value;
            HostAPIKeyValue = App.HostAPIKey.Value;
            HostUsernameValue = App.HostUsername.Value;
            HostPasswordValue = App.HostPassword.Value;

            settingsDbService = serviceProvider.GetService<SettingsDbService>();
        }

        [RelayCommand]
        async Task SaveSettings()
        {
            App.HostMode.Value = HostModeValue;
            App.HostAddress.Value = HostAddressValue;
            App.HostAPIKey.Value = HostAPIKeyValue;
            App.HostUsername.Value = HostUsernameValue;
            App.HostPassword.Value = HostPasswordValue;

            await settingsDbService.UpdateAsync(App.HostMode);
            await settingsDbService.UpdateAsync(App.HostAddress);
            await settingsDbService.UpdateAsync(App.HostAPIKey);
            await settingsDbService.UpdateAsync(App.HostUsername);
            await settingsDbService.UpdateAsync(App.HostPassword);
        }

        [RelayCommand]
        async Task ResetSettings()
        {
            HostModeValue = App.HostMode.Value;
            HostAddressValue = App.HostAddress.Value;
            HostAPIKeyValue = App.HostAPIKey.Value;
            HostUsernameValue = App.HostUsername.Value;
            HostPasswordValue = App.HostPassword.Value;
        }
    }
}
