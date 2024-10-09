using Collectiv.Abstracts;
using Collectiv.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Collectiv.Models
{
    public partial class Setting : Entity, INamable
    {
        [Required]
        [ObservableProperty]
        public string name;

        [Required]
        [ObservableProperty]
        public string value;
    }
}