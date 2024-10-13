using Collectiv.Abstracts;
using Collectiv.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.Services
{
    public  class SettingsDbService(IServiceProvider serviceProvider, SettingsDbContext settingsDbContext)
        : DatabaseService<SettingsDbContext>(serviceProvider, settingsDbContext)
    {
        
    }
}
