using Collectiv.Abstracts;
using Collectiv.Abstracts;
using Collectiv.Interfaces;
using Collectiv.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv.Services
{
    public class ApplicationDbService(IServiceProvider serviceProvider, ApplicationDbContext applicationDbContext)
        : DatabaseService<ApplicationDbContext>(serviceProvider, applicationDbContext)
    {
        public async virtual Task SetFilePackagePrimacyAsync(FilePackage filePackage)
        {
            try
            {
                if (filePackage.IsPrimary)
                {
                    // Clear all other sibling primacy states before setting the new primary
                    foreach (var entity in dbContext.Set<FilePackage>().Where(x => x.ContainerId == filePackage.ContainerId && x.Id != filePackage.Id))
                    {
                        entity.IsPrimary = false;
                    }
                }
                await dbContext.SaveChangesAsync();
            }
            catch
            {
                // TODO: Surface error to user or otherwise log
                return;
            }
        }

        public async virtual Task SetFilePrimacyAsync(Models.File file)
        {
            try
            {
                if (file.IsPrimary)
                {
                    // Clear all other sibling primacy states before setting the new primary
                    foreach (var entity in dbContext.Set<Models.File>().Where(x => x.FilePackageId == file.FilePackageId && x.Id != file.Id))
                    {
                        entity.IsPrimary = false;
                    }
                }

                var primaryFile = await dbContext.Set<Models.File>().SingleOrDefaultAsync(x => x.Id == file.Id);
                if(primaryFile is null)
                {
                    return;
                }

                primaryFile.IsPrimary = file.IsPrimary;

                await dbContext.SaveChangesAsync();
            }
            catch
            {
                // TODO: Surface error to user or otherwise log
                return;
            }
        }
    }
}
