using Collectiv.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.Common.DTOs
{
    public class FilePackageDTO
    {
        public Guid ContainerId { get; set; }
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<FileDTO> Files { get; set; }

        public FilePackageDTO()
        {
            Files = new List<FileDTO>();
        }
    }
}
