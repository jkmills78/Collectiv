using Collectiv.Abstracts;
using Collectiv.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Collectiv.Models
{
    public partial class Attribute : Entity, INamable
    {
        [ObservableProperty]
        private Guid containerId;

        [ObservableProperty]
        private int sequence;

        [Required]
        [ObservableProperty]
        private string name;

#nullable enable
        [ObservableProperty]
        private string? value;
#nullable disable

        private Container container;
        public virtual Container Container { get => container; set { container = value; OnPropertyChanged(); } }
    }
}
