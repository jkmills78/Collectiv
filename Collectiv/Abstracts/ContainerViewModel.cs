using Collectiv.Abstracts;
using Collectiv.ContentPages;
using Collectiv.Models;
using Collectiv.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.Abstracts
{
    public abstract partial class ContainerViewModel<T> : ViewModel
    {
        #region Backing Model

        [ObservableProperty]
        private Container container;

        #endregion

        [ObservableProperty]
        private FileViewModel coverImage;

        [ObservableProperty]
        private SelectableObservableCollection<AttributeViewModel> attributeViewModels;

        [ObservableProperty]
        private ICollection<FilePackageViewModel> filePackageViewModels;

        protected string oldName;

        protected Action<ContainerViewModel<T>> cancel;
        protected Func<ContainerViewModel<T>, Task> remove;

        protected ContainerViewModel(IServiceProvider serviceProvider, Container container, Action<ContainerViewModel<T>> cancel, Func<ContainerViewModel<T>, Task> remove)
            : base(serviceProvider)
        {
            AttributeViewModels = new SelectableObservableCollection<AttributeViewModel>();
            FilePackageViewModels = new ObservableCollection<FilePackageViewModel>();
            Container = container;
            this.cancel = cancel;
            this.remove = remove;

            oldName = container.Name;
            Container.PropertyChanged += Container_PropertyChanged;
        }

        private void Container_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Container.Name))
            {
                oldName = (sender as Container)?.Name;
            }
        }

        private void FilePackageViewModel_CoverImageChanged(object sender, EventArgs e)
        {
            if (sender is FilePackageViewModel coverImage)
            {
                var filePackageViewModel = FilePackageViewModels.SingleOrDefault(x => x.FilePackage.Id == coverImage.FilePackage.Id);
                if (filePackageViewModel is null)
                {
                    return;
                }

                Task.Run(() => applicationDbService.SetFilePackagePrimacyAsync(filePackageViewModel.FilePackage)).Wait();
                Task.Run(LoadCoverImage).Wait();
            }
        }

        private void FileViewModel_CoverImageChanged(object sender, EventArgs e)
        {
            if (sender is FileViewModel coverImage)
            {
                if (coverImage.File.IsPrimary)
                {
                    var filePackageViewModel = FilePackageViewModels.SingleOrDefault(x => x.FilePackage.Id == coverImage.File.FilePackageId);
                    var fileViewModel = filePackageViewModel.FileViewModels.SingleOrDefault(x => x.File.Id == coverImage.File.Id);
                    if (fileViewModel is null)
                    {
                        return;
                    }

                    foreach (var file in filePackageViewModel.FilePackage.Files.Where(x => x.Id != coverImage.File.Id))
                    {
                        file.IsPrimary = false;
                    }

                    Task.Run(() => applicationDbService.SetFilePrimacyAsync(fileViewModel.File)).Wait();
                    Task.Run(LoadCoverImage).Wait();
                }
            }
        }

        #region Commands

        [RelayCommand]
        async Task RemoveContainer()
        {
            await remove(this);
            await Shell.Current.GoToAsync("../..", true);
        }

        [RelayCommand]
        async Task CancelContainer()
        {
            await Cancel();
        }

        public async Task Cancel()
        {
            if (await applicationDbService.ExistsAsync<Container>(Container.Id))
            {
                Container.Name = oldName;
            }
            else
            {
                cancel(this);
            }
            IsConfirmed = true;
        }

        [RelayCommand]
        async Task EditContainerName()
        {
            IsConfirmed = !IsConfirmed;
        }

        [RelayCommand]
        async Task AddFilePackage()
        {
            var filePackage = new FilePackage()
            {
                Id = Guid.NewGuid(), // Get the next Id and assign
                ContainerId = Container.Id,
                Container = Container,
                Sequence = FilePackageViewModels.Select(viewModel => viewModel?.FilePackage?.Sequence).Max() + 1 ?? 1
            };

            var filePackageViewModel = new FilePackageViewModel(serviceProvider, filePackage);
            filePackageViewModel.CoverImageChanged += FilePackageViewModel_CoverImageChanged;

            await Shell.Current.GoToAsync(nameof(FilePackageDetails), true, new Dictionary<string, object>
            {
                { "FilePackageViewModel", filePackageViewModel }
            });
        }

        [RelayCommand]
        async Task EditFilePackage(FilePackageViewModel filePackageViewModel)
        {
            await Shell.Current.GoToAsync(nameof(FilePackageDetails), true, new Dictionary<string, object>
            {
                { "FilePackageViewModel", filePackageViewModel }
            });
        }

        [RelayCommand]
        async Task ConfirmContainer()
        {
            if (await applicationDbService.ExistsAsync<Container>(Container.Id))
            {
                await applicationDbService.UpdateAsync(Container);
            }
            else
            {
                await applicationDbService.AddAsync(Container);
            }
            IsConfirmed = true;
        }

        #endregion

        #region Loaders

        public async Task LoadCoverImage()
        {
            var primaryFilePackage = Container.FilePackages.SingleOrDefault(x => x.IsPrimary);
            if (primaryFilePackage == null)
            {
                CoverImage = null;
                return;
            }

            var file = primaryFilePackage.Files.SingleOrDefault(x => x.IsPrimary);
            if (file == null)
            {
                return;
            }

            byte[] fileData;
            if (App.HostMode.Value == "Hosted")
            {
                fileData = await restService.GetFileAsync(file.FilePackage.ContainerId, file.FilePackage.Id, Path.GetFileName(file.FullPath)).ConfigureAwait(false);
            }
            else
            {
                fileData = System.IO.File.ReadAllBytes(file.FullPath);
            }

            CoverImage = new FileViewModel(serviceProvider, file) { FileData = fileData };
        }

        public void LoadFilePackages()
        {
            FilePackageViewModels.Clear();

            if (Container != null)
            {
                foreach (var filePackage in Container.FilePackages)
                {
                    var filePackageViewModel = new FilePackageViewModel(serviceProvider, filePackage) { IsConfirmed = true };
                    foreach (var file in filePackage.Files)
                    {
                        byte[] fileData;
                        if (App.HostMode.Value == "Hosted")
                        {
                            fileData = Task.Run(() => restService.GetFileAsync(filePackage.ContainerId, filePackage.Id, Path.GetFileName(file.FullPath))).Result;
                        }
                        else
                        {
                            fileData = System.IO.File.ReadAllBytes(file.FullPath);
                        }

                        var fileViewModel = new FileViewModel(serviceProvider, file) { FileData = fileData };
                        fileViewModel.CoverImageChanged += FileViewModel_CoverImageChanged;
                        filePackageViewModel.FileViewModels.Add(fileViewModel);

                        if (file.IsPrimary)
                        {
                            filePackageViewModel.CoverImage = fileViewModel;
                            fileViewModel.File.IsPrimary = file.IsPrimary;

                            if (filePackage.IsPrimary)
                            {
                                CoverImage = fileViewModel;
                                filePackageViewModel.FilePackage.IsPrimary = filePackage.IsPrimary;
                            }
                        }
                    }
                    filePackageViewModel.CoverImageChanged += FilePackageViewModel_CoverImageChanged;
                    FilePackageViewModels.Add(filePackageViewModel);
                };
            }
        }

        #endregion
    }
}
