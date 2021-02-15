using Hangfire;
using Images.Caching.Configuration;
using Images.EntityFrameworkContext;
using Images.Repository.DataContracts;
using Images.Services.DataContracts.Caching;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Images.Repository.Cached
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly static CacheProviderType cacheTech = CacheProviderType.InMemory;
        private readonly string cacheKey = $"{typeof(T)}";
        private readonly DatabaseContext _dbContext;
        private readonly Func<CacheProviderType, ICacheService> _cacheService;

        public GenericRepository(DatabaseContext dbContext, Func<CacheProviderType, ICacheService> cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public virtual async Task<T> FindByIdAsync(long id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (!_cacheService(cacheTech).TryGet(cacheKey, out IReadOnlyList<T> cachedList))
            {
                cachedList = await _dbContext.Set<T>().AsNoTracking().ToListAsync();
                _cacheService(cacheTech).Set(cacheKey, cachedList);
            }
            return cachedList;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            BackgroundJob.Enqueue(() => RefreshCache());
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task RefreshCache()
        {
            _cacheService(cacheTech).Remove(cacheKey);
            var cachedList = await _dbContext.Set<T>().ToListAsync();
            _cacheService(cacheTech).Set(cacheKey, cachedList);
        }
    }
}
