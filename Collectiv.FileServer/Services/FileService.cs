using Collectiv.FileServer.Interfaces;

namespace Collectiv.FileServer.Services
{
    public class FileService : IFileService
    {
        private const string baseDir = "/UserData";

        public byte[] GetFile(Guid containerId, Guid packageId, string fileName)
        {
            var fullPath = Path.Combine(baseDir, containerId.ToString(), packageId.ToString(), fileName);
            if (File.Exists(fullPath))
            {
                return File.ReadAllBytes(fullPath);
            }

            return Array.Empty<byte>();
        }

        public bool AddOrUpdateFilePackage(FilePackageDTO filePackageDTO)
        {
            try
            {
                // Bad data
                if (filePackageDTO.Id == Guid.Empty
                    || (filePackageDTO.ContainerId == Guid.Empty)
                    || filePackageDTO.Files.Any(file => file.FileData is null)
                    || filePackageDTO.Files.Any(file => file.FileData.Length <= 0))
                {
                    return false;
                }

                string directory = baseDir;

                directory = Path.Combine(directory, $"{filePackageDTO.ContainerId}", $"{filePackageDTO.Id}");

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                else
                {
                    // The directory already exists, so make sure files that are no longer in the package get deleted
                    foreach (var fileName in Directory.EnumerateFiles(directory))
                    {
                        if (filePackageDTO.Files.Any(file => file.FileName == fileName))
                        {
                            continue;
                        }
                        DeleteFile(Path.Combine(directory, fileName));
                    }
                }

                foreach (var fileDTO in filePackageDTO.Files)
                {
                    if (!AddFile(directory, fileDTO))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddFile(string directory, FileDTO fileDTO)
        {
            try
            {
                var fullPath = Path.Combine(directory, fileDTO.FileName);

                var fileStream = new FileStream(fullPath, FileMode.Create);
                var memoryStream = new MemoryStream(fileDTO.FileData);
                memoryStream.CopyTo(fileStream);
                fileStream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteFile(Guid containerId, Guid packageId, string fileName)
        {
            var fullPath = Path.Combine(baseDir, containerId.ToString(), packageId.ToString(), fileName);
            return DeleteFile(fullPath);
        }

        public bool DeleteFile(string fullPath)
        {
            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryDeleteDirectory(Guid containerId, Guid? packageId = null, bool recursive = false)
        {
            string directory;
            directory = Path.Combine(baseDir, containerId.ToString(), packageId?.ToString() ?? string.Empty);
            return TryDeleteDirectory(directory, recursive);
        }

        public bool TryDeleteDirectory(string directory, bool recursive = false)
        {
            if (directory == "/UserData")
            {
                return false;
            }

            if (Directory.Exists(directory))
            {
                try
                {
                    Directory.Delete(directory, recursive);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
