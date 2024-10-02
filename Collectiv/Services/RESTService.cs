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
        public async Task<HttpResponseMessage> PostFilePackageAsync(FilePackageDTO filePackageDTO)
        {
            using (HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(App.HostAddress.Value) })
            {
                string serializedJsonFileData = JsonSerializer.Serialize(filePackageDTO);
                return await httpClient.PostAsync("/api/FilePackage", new StringContent(serializedJsonFileData, Encoding.UTF8, "application/json"));
            }
        }

        public async Task<byte[]> DeleteFilePackageAsync(Guid containerId, Guid packageId)
        {
            using (HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(App.HostAddress.Value) })
            {
                using HttpResponseMessage response = await httpClient.DeleteAsync($"/api/FilePackage?containerId={containerId}&packageId={packageId}");
                return await response.Content.ReadAsByteArrayAsync();
            }
        }

        public async Task<byte[]> GetFileAsync(Guid containerId, Guid packageId, string fileName)
        {
            using (HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(App.HostAddress.Value) })
            {
                using HttpResponseMessage response = await httpClient.GetAsync($"/api/File?containerId={containerId}&packageId={packageId}&fileName={fileName}");
                return await response.Content.ReadAsByteArrayAsync();
            }
        }

        //public async Task<HttpResponseMessage> PostAsync(HttpClient httpClient, JsonFileData jsonFileData)
        //{
        //    string serializedJsonFileData = JsonSerializer.Serialize(jsonFileData);
        //    return await httpClient.PostAsync("/api/File", new StringContent(serializedJsonFileData, Encoding.UTF8, "application/json"));
        //}

        public async Task<HttpResponseMessage> PostFileAsync(FilePackageDTO filePackageDTO)
        {
            using (HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(App.HostAddress.Value) })
            {
                string serializedJsonFileData = JsonSerializer.Serialize(filePackageDTO);
                return await httpClient.PostAsync("/api/File", new StringContent(serializedJsonFileData, Encoding.UTF8, "application/json"));
            }
        }

        public async Task<byte[]> DeleteFileAsync(Guid containerId, Guid packageId, string fileName)
        {
            using (HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(App.HostAddress.Value) })
            {
                using HttpResponseMessage response = await httpClient.DeleteAsync($"/api/File?containerId={containerId}&packageId={packageId}&fileName={fileName}");
                return await response.Content.ReadAsByteArrayAsync();
            }
        }
    }
}


























//using Collectiv.Interfaces;
//using Microsoft.Maui.Storage;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace Collectiv.Services
//{
//    public class RESTService : IRESTService
//    {
//        private const int maxRetries = 5;

//        public async Task<HttpResponseMessage> PostFilePackageAsync(HttpClient httpClient, FilePackageDTO filePackageDTO)
//        {
//            string serializedJsonFileData = JsonSerializer.Serialize(filePackageDTO);

//            HttpResponseMessage response = new HttpResponseMessage();
//            int currentRetries = 0;
//            do
//            {
//                Thread.Sleep(5000);
//                response = await httpClient.PostAsync("/api/FilePackage", new StringContent(serializedJsonFileData, Encoding.UTF8, "application/json"));
//                currentRetries++;

//            }
//            while (!response.IsSuccessStatusCode && currentRetries < maxRetries); // Retry for ~30 seconds
//            return response;
//        }

//        public async Task<HttpResponseMessage> DeleteFilePackageAsync(HttpClient httpClient, int containerId, int packageId)
//        {
//            HttpResponseMessage response = new HttpResponseMessage();
//            int currentRetries = 0;
//            do
//            {
//                Thread.Sleep(5000);
//                response = await httpClient.DeleteAsync($"/api/FilePackage?containerId={containerId}&packageId={packageId}");
//                currentRetries++;

//            }
//            while (!response.IsSuccessStatusCode && currentRetries < maxRetries); // Retry for ~30 seconds
//            return response;
//        }

//        //public async Task<byte[]> GetFileAsync(HttpClient httpClient, int containerId, int packageId, string fileName)
//        //{
//        //    using HttpResponseMessage response = await httpClient.GetAsync($"/api/File?containerId={containerId}&packageId={packageId}&fileName={fileName}");
//        //    return await response.Content.ReadAsByteArrayAsync();
//        //}

//        public async Task<byte[]> GetFileAsync(HttpClient httpClient, int containerId, int packageId, string fileName)
//        {
//            HttpResponseMessage response = new HttpResponseMessage();
//            int currentRetries = 0;
//            do
//            {
//                Thread.Sleep(5000);
//                response = await httpClient.GetAsync($"/api/File?containerId={containerId}&packageId={packageId}&fileName={fileName}");
//                currentRetries++;

//            }
//            while (!response.IsSuccessStatusCode && currentRetries < maxRetries); // Retry for ~30 seconds
//            return await response.Content.ReadAsByteArrayAsync();
//        }

//        //public async Task<HttpResponseMessage> PostAsync(HttpClient httpClient, JsonFileData jsonFileData)
//        //{
//        //    string serializedJsonFileData = JsonSerializer.Serialize(jsonFileData);
//        //    return await httpClient.PostAsync("/api/File", new StringContent(serializedJsonFileData, Encoding.UTF8, "application/json"));
//        //}

//        public async Task<HttpResponseMessage> PostFileAsync(HttpClient httpClient, FilePackageDTO filePackageDTO)
//        {
//            string serializedJsonFileData = JsonSerializer.Serialize(filePackageDTO);

//            HttpResponseMessage response = new HttpResponseMessage();
//            int currentRetries = 0;
//            do
//            {
//                Thread.Sleep(5000);
//                response = await httpClient.PostAsync("/api/File", new StringContent(serializedJsonFileData, Encoding.UTF8, "application/json"));
//                currentRetries++;

//            }
//            while (!response.IsSuccessStatusCode && currentRetries < maxRetries); // Retry for ~30 seconds
//            return response;
//        }

//        public async Task<HttpResponseMessage> DeleteFileAsync(HttpClient httpClient, int containerId, int packageId, string fileName)
//        {
//            HttpResponseMessage response = new HttpResponseMessage();
//            int currentRetries = 0;
//            do
//            {
//                Thread.Sleep(5000);
//                response = await httpClient.DeleteAsync($"/api/File?containerId={containerId}&packageId={packageId}&fileName={fileName}");
//                currentRetries++;

//            }
//            while (!response.IsSuccessStatusCode && currentRetries < maxRetries); // Retry for ~30 seconds
//            return response;
//        }
//    }
//}

