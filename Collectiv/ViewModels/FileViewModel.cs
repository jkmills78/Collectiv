using Collectiv.Abstracts;
using ByteSizeLib;
using File = Collectiv.Models.File;
using System.ComponentModel;

namespace Collectiv.ViewModels
{
    public partial class FileViewModel : ViewModel
    {
        [ObservableProperty]
        private bool isImageFile;

        [ObservableProperty]
        private File file;

        [ObservableProperty]
        private string fileName;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FileSize))]
        private byte[] fileData;

        public string FileSize => ByteSize.FromBytes(FileData?.Length ?? 0).ToString();

        public event EventHandler CoverImageChanged;

        public FileViewModel(IServiceProvider serviceProvider, File file)
            : base(serviceProvider)
        {
            File = file;
            FileName = Path.GetFileName(file.FullPath);
            if (File != null)
            {
                if (File.MimeType.StartsWith("image"))
                {
                    IsImageFile = true;
                }
            }

            File.PropertyChanged += File_PropertyChanged;
        }

        private void File_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsPrimary")
            {
                CoverImageChanged?.Invoke(this, null);
            }
        }
    }
}
