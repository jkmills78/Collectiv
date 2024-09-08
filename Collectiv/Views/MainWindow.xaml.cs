using Collectiv;
using Collectiv.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Collectiv.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CollectionViewSource CollectionsViewSource;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            CollectionsViewSource = (CollectionViewSource)FindResource(nameof(CollectionsViewSource));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(var collectionViewModel in ((MainWindowViewModel)DataContext).CollectionViewModels)
            {
                collectionViewModel.RefreshCollectionList = CollectionList.Items.Refresh;
            }

            // bind to the source
            CollectionsViewSource.Source = ((MainWindowViewModel)DataContext).CollectionViewModels;
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    // all changes are automatically tracked, including
        //    // deletes!
        //    _context.SaveChanges();

        //    // this forces the grid to refresh to latest values
        //    categoryDataGrid.Items.Refresh();
        //    productsDataGrid.Items.Refresh();
        //}

        protected override void OnClosing(CancelEventArgs e)
        {
            // clean up database connections
            App.DbContext.Dispose();

            base.OnClosing(e);
        }
    }
}