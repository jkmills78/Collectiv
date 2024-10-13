using Collectiv.Abstracts;
using Collectiv.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectiv.Models
{
    public partial class File : Entity
    {
        [ObservableProperty]
        private Guid filePackageId;

        [ObservableProperty]
        private bool isPrimary;

        [ObservableProperty]
        private int sequence;

        [Required]
        [ObservableProperty]
        private string fullPath;

        [Required]
        [ObservableProperty]
        private string mimeType;

        private FilePackage filePackage;
        public virtual FilePackage FilePackage { get => filePackage; set { filePackage = value; OnPropertyChanged(); } }
    }
}
