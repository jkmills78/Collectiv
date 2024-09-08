using Collectiv.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Collectiv.UserControls
{
    /// <summary>
    /// Interaction logic for CollectionSettings.xaml
    /// </summary>
    public partial class CollectionSettings : UserControl
    {
        private CollectionViewSource AttributesViewSource;

        public CollectionSettings()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // bind to the source
            AttributesViewSource.Source = ((CollectionViewModel)DataContext).AvailableAttributeViewModels;
        }
    }
}
