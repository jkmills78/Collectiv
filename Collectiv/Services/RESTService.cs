using Collectiv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Collectiv.Services
{
    public class RESTService : IRESTService
    {
        HttpClient httpClient;

        public RESTService(IServiceProvider serviceProvider)
        {
            httpClient = serviceProvider.GetService<HttpClient>();
            httpClient.BaseAddress = new Uri(App.HostAddress.Value);
        }

        public async Task<byte[]> GetFileAsync(Guid containerId, Guid packageId, string fileName)
        {
            using HttpResponseMessage response = await httpClient.GetAsync($"/api/File?containerId={containerId}&packageId={packageId}&fileName={fileName}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                return Array.Empty<byte>();
            }
        }

        public async Task<HttpResponseMessage> PostFilePackageAsync(FilePackageDTO filePackageDTO)
        {
            string serializedJsonFileData = JsonSerializer.Serialize(filePackageDTO);
            return await httpClient.PostAsync("/api/File", new StringContent(serializedJsonFileData, Encoding.UTF8, "application/json"));
        }

        public async Task<HttpResponseMessage> DeleteFileAsync(Guid containerId, Guid packageId, string fileName)
        {
            return await httpClient.DeleteAsync($"/api/File?containerId={containerId}&packageId={packageId}&fileName={fileName}");
        }

        public async Task<HttpResponseMessage> DeleteFilePackageAsync(Guid containerId, Guid packageId)
        {
            return await httpClient.DeleteAsync($"/api/File?containerId={containerId}&packageId={packageId}");
        }

        public async Task<HttpResponseMessage> DeleteFilePackagesAsync(Guid containerId)
        {
            return await httpClient.DeleteAsync($"/api/File?containerId={containerId}");
        }
    }
}