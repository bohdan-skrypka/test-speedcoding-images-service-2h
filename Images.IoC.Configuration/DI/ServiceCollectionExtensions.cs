using AspNetCoreRateLimit;
using AutoMapper;
using Hangfire;
using Images.Caching.Configuration;
using Images.Caching.Services.Distributed;
using Images.Caching.Services.Local;
using Images.EntityFrameworkContext;
using Images.Repository.Cached;
using Images.Repository.DataContracts;
using Images.Services;
using Images.Services.Contracts;
using Images.Services.DataContracts.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Cachable;
using System;
using System.Reflection;

namespace Images.IoC.Configuration.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services != null)
            {
                services.AddTransient<ISearchService, SearchService>();
            }
        }

        public static void ConfigureMappings(this IServiceCollection services)
        {
            if (services != null)
            {
                //Automap settings
                services.AddAutoMapper(Assembly.GetExecutingAssembly());
            }
        }

        public static void RegisterHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            var connectString = configuration.GetConnectionString("DefaultConnection");

            services.AddHangfire(x => x.UseSqlServerStorage(connectString));
            services.AddHangfireServer();
        }

        public static void RegisterRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName)));

            #region Repositories with cached results for Get operations
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IImagesRepositoryCached, ImagesRepositoryCached>();
            #endregion
        }

        public static void ConfigureCachingStrategy(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheConfiguration>(configuration.GetSection("CacheConfiguration"));
            services.AddMemoryCache();
            services.AddTransient<InMemoryCacheService>();
            services.AddTransient<RedisCacheService>();

            services.AddTransient<Func<CacheProviderType, ICacheService>>(ServiceProvider => key =>
            {
                switch (key)
                {
                    case CacheProviderType.Redis:
                        return ServiceProvider.GetService<RedisCacheService>();
                    case CacheProviderType.InMemory:
                        return ServiceProvider.GetService<InMemoryCacheService>();
                    default:
                        return ServiceProvider.GetService<InMemoryCacheService>();
                }
            });
        }


        public static void RegisterRateLimits(this IServiceCollection services, IConfiguration configuration)
        {
            // needed to load configuration from appsettings.json
            services.AddOptions();

            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            //load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

            //load ip rules from appsettings.json
            services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));

            // inject counter and rules stores
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            // Add framework services.
            services.AddMvc();

            // https://github.com/aspnet/Hosting/issues/793
            // the IHttpContextAccessor service is not registered by default.
            // the clientId/clientIp resolvers use it.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
    }
}
