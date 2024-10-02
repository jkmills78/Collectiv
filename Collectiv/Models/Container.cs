using Collectiv.Bases;
using Collectiv.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Collectiv.Interfaces.IContainer;

namespace Collectiv.Models
{
    public partial class Container : Entity, INamable, IContainer
    {
#nullable enable
        [ObservableProperty]
        private Guid? parentId;
#nullable disable

        [ObservableProperty]
        private int sequence;

        [Required]
        [ObservableProperty]
        private ContainerType type;

        [Required]
        [ObservableProperty]
        private string name;

#nullable enable
        [ObservableProperty]
        private string? description;
#nullable disable

        private ICollection<Container> children;
        public virtual ICollection<Container> Children { get => children; set { children = value; OnPropertyChanged(); } }

        private ICollection<Attribute> attributes;
        public virtual ICollection<Attribute> Attributes { get => attributes; set { attributes = value; OnPropertyChanged(); } }

        private ICollection<FilePackage> filePackages;
        public virtual ICollection<FilePackage> FilePackages { get => filePackages; set { filePackages = value; OnPropertyChanged(); } }

        private Container parent;
        public virtual Container Parent { get => parent; set { parent = value; OnPropertyChanged(); } }

        public Container()
        {
            Children = new ObservableCollection<Container>();
            Attributes = new ObservableCollection<Attribute>();
            FilePackages = new ObservableCollection<FilePackage>();
        }
    }
}
