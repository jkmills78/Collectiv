using Collectiv.ContentPages;
using Collectiv.Interfaces;
using Collectiv.Models;
using Collectiv.Services;
using Collectiv.ViewModels;
using Microsoft.Extensions.Logging;

namespace Collectiv
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .RegisterDbContexts()
                .RegisterServices()
                .RegisterModels()
                .RegisterViewModels()
                .RegisterContentPages()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", "FluentIconsRegular");
                    fonts.AddFont("fa-brands-400.ttf", "FABrands");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public static MauiAppBuilder RegisterDbContexts(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddDbContext<ApplicationDbContext>();
            mauiAppBuilder.Services.AddDbContext<SettingsDbContext>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<ILoggingService, LoggingService>();
            mauiAppBuilder.Services.AddTransient<IFileService, FileService>();
            mauiAppBuilder.Services.AddTransient<IRESTService, RESTService>();
            mauiAppBuilder.Services.AddTransient<ApplicationDbService>();
            mauiAppBuilder.Services.AddTransient<SettingsDbService>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterModels(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<Container>();
            mauiAppBuilder.Services.AddTransient<FilePackage>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainPageViewModel>();
            mauiAppBuilder.Services.AddSingleton<HostSettingsViewModel>();
            mauiAppBuilder.Services.AddTransient<CollectionViewModel>();
            mauiAppBuilder.Services.AddTransient<CollectionDetailsViewModel>();
            mauiAppBuilder.Services.AddTransient<CollectionSettingsViewModel>();
            mauiAppBuilder.Services.AddTransient<ItemDetailsViewModel>();
            mauiAppBuilder.Services.AddTransient<FilePackageViewModel>();
            mauiAppBuilder.Services.AddTransient<FilePackageDetailsViewModel>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterContentPages(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainPage>();
            mauiAppBuilder.Services.AddSingleton<HostSettings>();
            mauiAppBuilder.Services.AddTransient<CollectionDetails>();
            mauiAppBuilder.Services.AddTransient<CollectionSettings>();
            mauiAppBuilder.Services.AddTransient<ItemDetails>();
            mauiAppBuilder.Services.AddTransient<FilePackageDetails>();

            return mauiAppBuilder;
        }
    }
}
