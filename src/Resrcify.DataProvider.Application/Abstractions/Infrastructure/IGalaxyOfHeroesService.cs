using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Resrcify.DataProvider.Application.Abstractions.Infrastructure;

public interface IGalaxyOfHeroesService
{
    Task<HttpResponseMessage> GetGameData(string? version = null, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> GetLocalization(string? version = null, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> GetMetadata(string? version = null, CancellationToken cancellationToken = default);
}
