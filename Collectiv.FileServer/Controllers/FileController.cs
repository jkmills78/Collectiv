using Collectiv.Common.DTOs;
using Collectiv.FileServer.Interfaces;
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
        private IFileService fileService;

        public FileController(ILogger<FileController> logger, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;
            this.fileService = fileService;
        }

        // GET: api/<FileController>?containerId=1&packageId=1&fileName=image.jpg"
        [HttpGet]
        public IActionResult Get([FromQuery] Guid containerId, [FromQuery] Guid packageId, [FromQuery] string fileName)
        {
            byte[] file = fileService.GetFile(containerId, packageId, fileName);
            if (file != Array.Empty<byte>())
            {
                var Inspector = new ContentInspectorBuilder()
                {
                    Definitions = MimeDetective.Definitions.Default.All()
                }.Build();

                var mimeType = Inspector.Inspect(file)[0].Definition.File.MimeType;
                if (mimeType != null)
                {
                    return File(file, mimeType, fileName);
                }
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

            return fileService.AddOrUpdateFilePackage(filePackageDTO) ? Ok() : BadRequest();
        }

        // DELETE: api/<FileController>?containerId=1&packageId=1&fileName=image.jpg"
        [HttpDelete]
        public bool Delete([FromQuery] Guid containerId, [FromQuery] Guid? packageId, [FromQuery] string fileName)
        {
            if (containerId != Guid.Empty)
            {
                if (packageId is not null && packageId != Guid.Empty)
                {
                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        bool isSuccess = fileService.DeleteFile(containerId, (Guid)packageId, fileName);

                        // Delete directories as they become empty
                        fileService.TryDeleteDirectory(containerId, packageId);
                        fileService.TryDeleteDirectory(containerId);

                        return isSuccess;
                    }

                    return fileService.TryDeleteDirectory(containerId, packageId, recursive: true);
                }

                return fileService.TryDeleteDirectory(containerId, recursive: true);
            }

            return false;

        }
    }
}
