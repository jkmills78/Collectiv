using Collectiv.Bases;
using Collectiv.ContentPages;
using Collectiv.Models;
using MimeDetective;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Collectiv.ViewModels
{
    public partial class FilePackageViewModel : ViewModel
    {
        [ObservableProperty]
        private FileViewModel coverImage;

        #region Backing Model

        [ObservableProperty]
        private FilePackage filePackage;

        #endregion

        [ObservableProperty]
        private ICollection<FileViewModel> fileViewModels;

        [ObservableProperty]
        private int fileCount;

        [ObservableProperty]
        private bool isMarkedForDeletion;

        public FilePackageViewModel(IServiceProvider serviceProvider, FilePackage filePackage)
            : base(serviceProvider)
        {
            FilePackage = filePackage;
            FileViewModels = new ObservableCollection<FileViewModel>();

            ((ObservableCollection<FileViewModel>)FileViewModels).CollectionChanged += FilePackageViewModel_CollectionChanged;
        }

        private void FilePackageViewModel_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            FileCount = FileViewModels.Count;
        }

        [RelayCommand]
        async Task AddFile()
        {
            var pickedFile = await fileService.PickAFile();
            if (pickedFile != null)
            {
                var fileStream = pickedFile.OpenReadAsync().Result;
                byte[] fileData = new byte[fileStream.Length];
                fileStream.Read(fileData, 0, fileData.Length);

                var Inspector = new ContentInspectorBuilder()
                {
                    Definitions = MimeDetective.Definitions.Default.All()
                }.Build();

                var mimeType = Inspector.Inspect(fileData)[0].Definition.File.MimeType;

                var file = new Models.File()
                {
                    Id = Guid.NewGuid(),
                    FilePackageId = FilePackage.Id,
                    FullPath = pickedFile.FullPath,
                    MimeType = mimeType
                };

                var fileViewModel = new FileViewModel(serviceProvider, file) { FileData = fileData };
                FileViewModels.Add(fileViewModel);
                FilePackage.Files.Add(fileViewModel.File);
            }
        }

        [RelayCommand]
        async Task RemoveFile(FileViewModel fileViewModel)
        {
            FileViewModels.Remove(fileViewModel);
        }

        [RelayCommand]
        async Task RemoveFilePackage()
        {
            IsMarkedForDeletion = true;
            IsConfirmed = false;
            await Shell.Current.GoToAsync("..", true, new Dictionary<string, object>
            {
                { "FilePackageViewModel", this }
            });
        }

        [RelayCommand]
        async Task SaveFilePackage()
        {
            IsMarkedForDeletion = false;
            IsConfirmed = true;
            await Shell.Current.GoToAsync("..", true, new Dictionary<string, object>
            {
                { "FilePackageViewModel", this }
            });
        }

        [RelayCommand]
        async Task CancelFilePackage()
        {
            IsMarkedForDeletion = false;
            IsConfirmed = false;
            await Shell.Current.GoToAsync("..", true, new Dictionary<string, object>
            {
                { "FilePackageViewModel", this }
            });
        }
    }
}
