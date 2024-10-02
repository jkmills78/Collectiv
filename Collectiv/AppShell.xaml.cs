using Collectiv.ContentPages;

namespace Collectiv
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(HostSettings), typeof(HostSettings));
            Routing.RegisterRoute(nameof(CollectionSettings), typeof(CollectionSettings));
            Routing.RegisterRoute(nameof(CollectionDetails), typeof(CollectionDetails));
            Routing.RegisterRoute(nameof(ItemDetails), typeof(ItemDetails));
            Routing.RegisterRoute(nameof(FilePackageDetails), typeof(FilePackageDetails));
        }
    }
}
