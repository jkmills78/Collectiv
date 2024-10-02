using Collectiv.Bases;
using ByteSizeLib;
using File = Collectiv.Models.File;

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
        }
    }
}
