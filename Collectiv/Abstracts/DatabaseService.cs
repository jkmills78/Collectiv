﻿using Collectiv.Abstracts;
using Collectiv.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IContainer = Collectiv.Interfaces.IContainer;

namespace Collectiv.Abstracts
{
    public abstract class DatabaseService<TContext> where TContext : DbContext
    {
        internal readonly TContext dbContext;

        public DatabaseService(IServiceProvider serviceProvider, TContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async virtual Task AddAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            try
            {
                if (!dbContext.Set<TEntity>().Any(db => db.Id == entity.Id))
                {
                    await dbContext.Set<TEntity>().AddAsync(entity);
                    dbContext.SaveChanges();
                }
            }
            catch
            {
                // TODO: Surface error to user or otherwise log
                return;
            }
        }
        public async virtual Task<TEntity> GetAsync<TEntity>(Guid id) where TEntity : Entity
        {
            return await dbContext.Set<TEntity>().SingleOrDefaultAsync(db => db.Id == id);
        }

        public async virtual Task<IQueryable<TEntity>> GetAsync<TEntity>() where TEntity : Entity
        {
            return dbContext.Set<TEntity>();
        }

        public async virtual Task<IQueryable<TEntity>> GetAsync<TEntity>(ContainerType type) where TEntity : Entity, IContainer
        {
            return dbContext.Set<TEntity>().Where(db => db.Type == type);
        }

        public async virtual Task UpdateAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            try
            {
                if (dbContext.Set<TEntity>().Any(db => db.Id == entity.Id))
                {
                    dbContext.Set<TEntity>().Update(entity);
                }
                await dbContext.SaveChangesAsync();
            }
            catch
            {
                // TODO: Surface error to user or otherwise log
                return;
            }
        }

        public async virtual Task RemoveAsync<TEntity>(Guid id) where TEntity : Entity
        {
            try
            {
                var entity = dbContext.Set<TEntity>().SingleOrDefault(db => db.Id == id);
                if (entity is not null)
                {
                    dbContext.Set<TEntity>().Remove(entity);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch
            {
                // TODO: Surface error to user or otherwise log
                return;
            }
        }

        public async virtual Task<bool> ExistsAsync<TEntity>(Guid id) where TEntity : Entity
        {
            return await dbContext.Set<TEntity>().AnyAsync(db => db.Id == id);
        }

        public async virtual Task CancelAllChangesAsync()
        {
            await Task.Run(() =>
            {
                foreach (var entry in dbContext.ChangeTracker.Entries())
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                    }
                }
            });
        }

        public async Task DestroyDatabaseAsync<TEntity>() where TEntity : Entity
        {
            try
            {
                await dbContext.Database.EnsureDeletedAsync();
            }
            catch
            {
                // TODO: Surface error to user or otherwise log
                return;
            }
        }

        public async Task InitializeAsync<TEntity>() where TEntity : Entity
        {
            try
            {
                await dbContext.Database.EnsureCreatedAsync();
                await dbContext.Set<TEntity>().LoadAsync();
            }
            catch
            {
                // TODO: Surface error to user or otherwise log
                return;
            }
        }
    }
}
