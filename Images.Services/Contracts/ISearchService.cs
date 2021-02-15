using Images.Services.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Images.Services.Contracts
{
    public interface ISearchService
    {
        Task<Image> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
