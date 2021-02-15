using Images.Caching.Configuration;
using Images.Database.Models.Business;
using Images.EntityFrameworkContext;
using Images.Repository.Cached;
using Images.Repository.DataContracts;
using Images.Services.DataContracts.Caching;
using Microsoft.EntityFrameworkCore;
using System;

namespace Repository.Cachable
{

    public class ImagesRepositoryCached : GenericRepository<ImageEntity>, IImagesRepositoryCached
    {
        private readonly DbSet<ImageEntity> _images;
        public ImagesRepositoryCached(DatabaseContext databaseContext, Func<CacheProviderType, ICacheService> cacheService) : base(databaseContext, cacheService)
        {
            _images = databaseContext.Set<ImageEntity>();
        }
    }
}
