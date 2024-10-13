using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.Common.DTOs
{
    public class FileDTO
    {
        public Guid FilePackageId { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public string MimeType { get; set; }
        public byte[] FileData { get; set; }
    }
}
