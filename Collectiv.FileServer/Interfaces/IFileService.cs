namespace Collectiv.FileServer.Interfaces
{
    public interface IFileService
    {
        byte[] GetFile(Guid containerId, Guid packageId, string fileName);
        bool AddFile(string directory, FileDTO fileDTO);
        bool AddOrUpdateFilePackage(FilePackageDTO filePackageDTO);
        bool DeleteFile(Guid containerId, Guid packageId, string fileName);
        bool DeleteFile(string fullPath);
        bool TryDeleteDirectory(Guid containerId, Guid? packageId = null, bool recursive = false);
        bool TryDeleteDirectory(string directory, bool recursive = false);
    }
}
