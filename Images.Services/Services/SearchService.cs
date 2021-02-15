using AutoMapper;
using Images.API.Common.Settings;
using Images.Services.Contracts;
using Images.Services.Model;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Images.Services
{
    public class SearchService : ISearchService
    {
        private AppSettings _settings;
        private readonly IMapper _mapper;

        public SearchService(IOptions<AppSettings> settings, IMapper mapper)
        {
            _settings = settings?.Value;
            _mapper = mapper;
        }

        public async Task<Image> GetAsync(string id)
        {
            return new Image
            {
                Id = id,
            };
        }
    }
}
