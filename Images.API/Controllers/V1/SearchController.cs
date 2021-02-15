using AutoMapper;
using Images.API.DataContracts;
using Images.API.DataContracts.Requests;
using Images.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using S = Images.Services.Model;

namespace Images.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/search")]
    [ApiController]
    public class SearchController : Controller
    {
        private readonly ISearchService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchController> _logger;

#pragma warning disable CS1591
        public SearchController(ISearchService service, IMapper mapper, ILogger<SearchController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }
#pragma warning restore CS1591

        #region GET
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Image))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Image))]
        [HttpGet("{searchTerm}")]
        public async Task<Image> Get(string searchTerm, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"SearchController::Get::{searchTerm}");

            var data = await _service.GetAsync(searchTerm, cancellationToken);

            if (data != null)
                return _mapper.Map<Image>(data);
            else
                return null;
        }
        #endregion
    }
}
