using Collectiv.Abstracts;
using Collectiv.Common.DTOs;
using Collectiv.ContentPages;
using Collectiv.Models;
using MimeDetective;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        private FilePackage oldFilePackage;

        public event EventHandler CoverImageChanged;

        public FilePackageViewModel(IServiceProvider serviceProvider, FilePackage filePackage)
            : base(serviceProvider)
        {
            FilePackage = filePackage;
            oldFilePackage = new FilePackage();
            CopyFilePackage(FilePackage, oldFilePackage);

            FileViewModels = new ObservableCollection<FileViewModel>();

            ((ObservableCollection<FileViewModel>)FileViewModels).CollectionChanged += FileViewModels_CollectionChanged;
            FilePackage.PropertyChanged += FilePackage_PropertyChanged;
        }

        private void FilePackage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsPrimary")
            {
                CoverImageChanged?.Invoke(this, null);
            }
        }

        private void FileViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (FileViewModel fileViewModel in e.NewItems)
                {
                    if (!FilePackage.Files.Any(file => file.Id == fileViewModel.File.Id))
                    {
                        FilePackage.Files.Add(fileViewModel.File);
                    }
                }
            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (FileViewModel fileViewModel in e.OldItems)
                {
                    if (FilePackage.Files.Any(file => file.Id == fileViewModel.File.Id))
                    {
                        FilePackage.Files.Remove(fileViewModel.File);
                    }
                }
            }

            FileCount = FileViewModels.Count;
        }

        private void FileViewModel_CoverImageChanged(object sender, EventArgs e)
        {
            if (sender is FileViewModel coverImage)
            {
                if (coverImage.File.IsPrimary)
                {
                    var fileViewModel = FileViewModels.SingleOrDefault(x => x.File.Id == coverImage.File.Id);
                    if (fileViewModel is null)
                    {
                        return;
                    }

                    foreach (var file in FilePackage.Files.Where(x => x.Id != coverImage.File.Id))
                    {
                        file.IsPrimary = false;
                    }

                    Task.Run(() => applicationDbService.SetFilePrimacyAsync(fileViewModel.File)).Wait();
                }
            }
        }

        [RelayCommand]
        async Task AddFile()
        {
            var pickedFile = await fileService.PickAFile();
            if (pickedFile != null)
            {
                var fileStream = await pickedFile.OpenReadAsync();
                byte[] fileData = new byte[fileStream.Length];
                fileStream.Read(fileData, 0, fileData.Length);

                var Inspector = new ContentInspectorBuilder()
                {
                    Definitions = MimeDetective.Definitions.Default.All()
                }.Build();

                var mimeType = Inspector.Inspect(fileData).FirstOrDefault()?.Definition.File.MimeType ?? "application/octet-stream";

                var file = new Models.File()
                {
                    Id = Guid.NewGuid(),
                    FilePackageId = FilePackage.Id,
                    FullPath = pickedFile.FullPath,
                    MimeType = mimeType,
                    Sequence = FileViewModels.Select(viewModel => viewModel?.File?.Sequence).Max() + 1 ?? 1
                };

                var fileViewModel = new FileViewModel(serviceProvider, file) { FileData = fileData };
                fileViewModel.CoverImageChanged += FileViewModel_CoverImageChanged;
                FileViewModels.Add(fileViewModel);
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
            await Shell.Current.GoToAsync("..", true, new Dictionary<string, object>
            {
                { "FilePackageViewModel", this }
            });
        }

        [RelayCommand]
        async Task SaveFilePackage()
        {
            if (await applicationDbService.ExistsAsync<FilePackage>(FilePackage.Id))
            {
                foreach (var file in FilePackage.Files)
                {
                    await applicationDbService.SetFilePrimacyAsync(file);

                    // Delete
                    foreach (var fileToDelete in (await applicationDbService.GetAsync<Models.File>()).Where(file => file.FilePackageId == FilePackage.Id))
                    {
                        if (FilePackage.Files.All(file => file.Id != fileToDelete.Id))
                        {
                            if (App.HostMode.Value == "Hosted")
                            {
                                await restService.DeleteFileAsync(fileToDelete.FilePackage.ContainerId, fileToDelete.FilePackageId, Path.GetFileName(file.FullPath));
                            }
                            await applicationDbService.RemoveAsync<Models.File>(fileToDelete.Id);
                        }
                    }

                    // Update
                    if (await applicationDbService.ExistsAsync<Models.File>(file.Id))
                    {
                        await applicationDbService.UpdateAsync(file);
                    }

                    // Add
                    if (!await applicationDbService.ExistsAsync<Models.File>(file.Id))
                    {
                        await applicationDbService.AddAsync(file);
                    }
                    await applicationDbService.UpdateAsync(FilePackage);
                }   
            }
            else
            {
                await applicationDbService.AddAsync(FilePackage);
            }

            var filePackageDto = new FilePackageDTO
            {
                ContainerId = FilePackage.ContainerId,
                Id = FilePackage.Id,
                Name = FilePackage.Name,
                Description = FilePackage.Description
            };

            foreach (var fileViewModel in FileViewModels)
            {
                filePackageDto.Files.Add(new FileDTO
                {
                    FilePackageId = fileViewModel.File.FilePackageId,
                    FileName = fileViewModel.FileName,
                    FullPath = fileViewModel.File.FullPath,
                    MimeType = fileViewModel.File.MimeType,
                    FileData = fileViewModel.FileData
                });
            }

            if (App.HostMode.Value == "Hosted")
            {
                await restService.PostFilePackageAsync(filePackageDto);
            }

            await Shell.Current.GoToAsync("..", true, new Dictionary<string, object>
            {
                { "FilePackageViewModel", this }
            });
        }

        [RelayCommand]
        async Task CancelFilePackage()
        {
            await Cancel();

            await Shell.Current.GoToAsync("..", true, new Dictionary<string, object>
            {
                { "FilePackageViewModel", this }
            });
        }

        public async Task Cancel()
        {
            await applicationDbService.CancelAllChangesAsync();
            CopyFilePackage(oldFilePackage, FilePackage);
            FileViewModels.Clear();
            foreach (var file in FilePackage.Files)
            {
                FileViewModels.Add(new FileViewModel(serviceProvider, file));
            }
        }

        private void CopyFilePackage(FilePackage source, FilePackage destination)
        {
            destination.Id = source.Id;
            destination.Container = source.Container;
            destination.ContainerId = source.ContainerId;
            destination.IsPrimary = source.IsPrimary;
            destination.Sequence = source.Sequence;
            destination.Name = source.Name;
            destination.Description = source.Description;

            destination.Files.Clear();

            foreach (var file in source.Files)
            {
                destination.Files.Add(file);
            }
        }
    }
}
