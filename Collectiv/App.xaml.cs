using Collectiv.Models;
using Collectiv.Services;

namespace Collectiv
{
    public partial class App : Application
    {
        public static Setting HostMode { get; set; }
        public static Setting HostAddress { get; set; }
        public static Setting HostAPIKey { get; set; }
        public static Setting HostUsername { get; set; }
        public static Setting HostPassword { get; set; }

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            var applicationDbService = serviceProvider.GetService<ApplicationDbService>();
            var settingsDbService = serviceProvider.GetService<SettingsDbService>();

            //applicationDbService.DestroyDatabaseAsync<Container>().Wait();
            //settingsDbService.DestroyDatabaseAsync<Container>().Wait();

            applicationDbService.InitializeAsync<Container>().Wait();
            settingsDbService.InitializeAsync<Setting>().Wait();

            ICollection<Setting> settings = settingsDbService.GetAsync<Setting>().Result.ToList();

            HostMode = settings.SingleOrDefault(setting => setting.Name == "HostMode");
            HostAddress = settings.SingleOrDefault(setting => setting.Name == "HostAddress");
            HostAPIKey = settings.SingleOrDefault(setting => setting.Name == "HostAPIKey");
            HostUsername = settings.SingleOrDefault(setting => setting.Name == "HostUsername");
            HostPassword = settings.SingleOrDefault(setting => setting.Name == "HostPassword");

            if(string.IsNullOrWhiteSpace(HostAddress.Value))
            {
                HostAddress.Value = "https://localhost:32771";
            }

            MainPage = new AppShell();
        }
    }
}
