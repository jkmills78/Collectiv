using Collectiv.Bases;
using Collectiv.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.Models
{
    public partial class FilePackage : Entity, INamable
    {
        [Required]
        [ObservableProperty]
        private Guid containerId;

        [ObservableProperty]
        private bool isPrimary;

        [ObservableProperty]
        private int sequence;

#nullable enable

        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private string? description;

#nullable disable

        private ICollection<File> files;
        public virtual ICollection<File> Files { get => files; set { files = value; OnPropertyChanged(); } }

        private Container container;
        public virtual Container Container { get => container; set { container = value; OnPropertyChanged(); } }

        public FilePackage()
        {
            Files = new ObservableCollection<File>();
        }
    }
}
