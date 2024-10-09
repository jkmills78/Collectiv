using Collectiv.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.Abstracts
{
    public abstract partial class Entity : ObservableValidator, IIdentifiable
    {
        [Key]
        [ObservableProperty]
        private Guid id;
    }
}
