using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Collectiv.Interfaces
{
    public interface IRESTService
    {
        Task<HttpResponseMessage> PostFilePackageAsync(FilePackageDTO filePackageDTO);
        Task<byte[]> DeleteFilePackageAsync(Guid containerId, Guid packageId);
        Task<byte[]> GetFileAsync(Guid containerId, Guid packageId, string fileName);
        Task<HttpResponseMessage> PostFileAsync(FilePackageDTO filePackageDTO);
        Task<byte[]> DeleteFileAsync(Guid containerId, Guid packageId, string fileName);
    }
}
