
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Resrcify.DataProvider.Application.Abstractions.Infrastructure;

namespace Resrcify.DataProvider.Infrastructure.HttpClients;

public class GalaxyOfHeroesService : IGalaxyOfHeroesService
{
    public HttpClient Client { get; }

    public GalaxyOfHeroesService(HttpClient client)
    {
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        Client = client;
    }

    public async Task<HttpResponseMessage> GetGameData(string? version = null, CancellationToken cancellationToken = default)
        => await Client.GetAsync("api/data/getgamedata", cancellationToken);

    public async Task<HttpResponseMessage> GetLocalization(string? version = null, CancellationToken cancellationToken = default)
        => await Client.GetAsync("api/data/getlocalizationdata", cancellationToken);

    public async Task<HttpResponseMessage> GetMetadata(string? version = null, CancellationToken cancellationToken = default)
       => await Client.GetAsync("api/content/getmetadata", cancellationToken);
}
