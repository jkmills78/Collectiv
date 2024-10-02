using Collectiv.Abstracts;
using Collectiv.Bases;
using Collectiv.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.Services
{
    public class ApplicationDbService(IServiceProvider serviceProvider, ApplicationDbContext applicationDbContext)
        : DatabaseService<ApplicationDbContext>(serviceProvider, applicationDbContext)
    {
    }
}
