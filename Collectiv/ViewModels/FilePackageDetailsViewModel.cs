using Collectiv.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.ViewModels
{
    [QueryProperty("FilePackageViewModel", "FilePackageViewModel")]
    public partial class FilePackageDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        private FilePackageViewModel filePackageViewModel;

        public FilePackageDetailsViewModel()
        {

        }
    }
}
