using Collectiv;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Collectiv
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly ApplicationDbContext DbContext = new ApplicationDbContext();
    }

}
