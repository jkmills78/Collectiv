using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Collectiv.Interfaces
{
    public interface IRESTService
    {
        Task<byte[]> GetFileAsync(Guid containerId, Guid packageId, string fileName);
        Task<HttpResponseMessage> PostFilePackageAsync(FilePackageDTO filePackageDTO);
        Task<HttpResponseMessage> DeleteFileAsync(Guid containerId, Guid packageId, string fileName);
        Task<HttpResponseMessage> DeleteFilePackageAsync(Guid containerId, Guid packageId);
        Task<HttpResponseMessage> DeleteFilePackagesAsync(Guid containerId);
    }
}
