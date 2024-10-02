using Collectiv.Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using MimeDetective;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Collectiv.FileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<FileController> logger;
        private const string baseDir = "/UserData";

        public FileController(ILogger<FileController> logger, IWebHostEnvironment webHostEnvironment)
        {
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: api/<FileController>?containerId=1&packageId=1&fileName=image.jpg"
        [HttpGet]
        public IActionResult Get([FromQuery] Guid containerId, [FromQuery] Guid packageId, [FromQuery] string fileName)
        {
            var fullPath = Path.Combine(baseDir, containerId.ToString(), packageId.ToString(), fileName);
            byte[] file = GetFile(fullPath);
            var Inspector = new ContentInspectorBuilder()
            {
                Definitions = MimeDetective.Definitions.Default.All()
            }.Build();

            var mimeType = Inspector.Inspect(file)[0].Definition.File.MimeType;
            if (file != null && mimeType != null)
            {
                return File(file, mimeType, fileName);
            }
            return BadRequest();
        }

        // POST: api/<FileController>
        [HttpPost]
        public IActionResult Post([FromBody] JsonObject serializedData)
        {
            if (serializedData is null)
            {
                return BadRequest();
            }

            FilePackageDTO? filePackageDTO = JsonSerializer.Deserialize<FilePackageDTO>(serializedData);
            if (filePackageDTO is null)
            {
                return BadRequest();
            }

            return AddOrUpdateFilePackage(filePackageDTO) ? Ok() : BadRequest();
        }

        // DELETE: api/<FileController>?containerId=1&packageId=1&fileName=image.jpg"
        [HttpDelete]
        public bool Delete([FromQuery] Guid containerId, [FromQuery] Guid packageId, [FromQuery] string fileName)
        {
            var fullPath = Path.Combine(baseDir, containerId.ToString(), packageId.ToString(), fileName);

            // Delete directories as they become empty

            TryDeleteDirectory(Path.Combine(baseDir, containerId.ToString(), packageId.ToString()));
            TryDeleteDirectory(Path.Combine(baseDir, containerId.ToString()));

            return DeleteFile(fullPath);
        }

        private bool TryDeleteDirectory(string directory)
        {
            if(directory == "/UserData")
            {
                return false;
            }

            if (!Directory.EnumerateFileSystemEntries(directory).Any())
            {
                try
                {
                    Directory.Delete(directory);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        private bool AddOrUpdateFilePackage(FilePackageDTO filePackageDTO)
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
                        if (filePackageDTO.Files.Any(file => Path.GetFileName(file.FullPath) == fileName))
                        {
                            continue;
                        }
                        DeleteFile(Path.Combine(directory, fileName));
                    }
                }

                foreach (var fileDTO in filePackageDTO.Files)
                {
                    if(!AddFile(directory, fileDTO))
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

        private byte[] GetFile(string fullPath)
        {
            return System.IO.File.ReadAllBytes(fullPath);
        }

        private bool AddFile(string directory, FileDTO fileDTO)
        {
            try
            {
                var fullPath = Path.Combine(directory, Path.GetFileName(fileDTO.FullPath));

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

        private bool DeleteFile(string fullPath)
        {
            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
