using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Collectiv.ViewModels
{
    public partial class AboutViewModel : ObservableObject
    {
        [ObservableProperty]
        public string version;

        public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

        public AboutViewModel()
        {
            version = AppInfo.VersionString;
        }
    }
}
