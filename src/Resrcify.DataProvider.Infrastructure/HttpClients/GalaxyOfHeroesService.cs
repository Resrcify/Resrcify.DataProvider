
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Resrcify.DataProvider.Application.Abstractions;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Resrcify.SharedKernel.Web.Extensions;
using Resrcify.DataProvider.Application.Models;

namespace Resrcify.DataProvider.Infrastructure.HttpClients;

public sealed class GalaxyOfHeroesService : IGalaxyOfHeroesService
{
    private readonly HttpClient _client;

    public GalaxyOfHeroesService(HttpClient client)
    {
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _client = client;
    }

    public async Task<Result<GameDataResponse>> GetGameData(CancellationToken cancellationToken = default)
    {
        using var response = await _client.GetAsync("api/data/getgamedata", cancellationToken);
        return await response.Convert<GameDataResponse>(cancellationToken: cancellationToken);
    }

    public async Task<Result<LocalizationBundleResponse>> GetLocalization(CancellationToken cancellationToken = default)
    {
        using var response = await _client.GetAsync("api/data/getlocalizationdata", cancellationToken);
        return await response.Convert<LocalizationBundleResponse>(cancellationToken: cancellationToken);
    }

    public async Task<Result<MetadataResponse>> GetMetadata(CancellationToken cancellationToken = default)
    {
        using var response = await _client.GetAsync("api/content/getmetadata", cancellationToken);
        return await response.Convert<MetadataResponse>(cancellationToken: cancellationToken);
    }

}
