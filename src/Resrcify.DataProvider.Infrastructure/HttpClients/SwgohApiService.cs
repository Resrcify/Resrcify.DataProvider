
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Resrcify.DataProvider.Application.Abstractions;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Resrcify.SharedKernel.Web.Extensions;
using Resrcify.DataProvider.Application.Models;
using System.Net.Http.Json;

namespace Resrcify.DataProvider.Infrastructure.HttpClients;

public sealed class SwgohApiService
    : ISwgohApiService
{
    private readonly HttpClient _client;

    public SwgohApiService(HttpClient client)
    {
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        _client = client;
    }

    public async Task<Result<GameDataResponse>> GetGameData(
        MetadataResponse metadataResponse,
        CancellationToken cancellationToken = default)
    {
        var request = new GameDataRequest
        {
            IncludePveUnits = false,
            Items = GetGameDataItemSum(),
            Version = metadataResponse.LatestGamedataVersion,
            RequestSegment = GameDataSegment.Gamedatasegmentall
        };
        using var response = await _client.PostAsJsonAsync(
            "api/game/getgamedata",
            new
            {
                Payload = request
            },
            cancellationToken);
        return await response.Convert<GameDataResponse>(
            cancellationToken: cancellationToken);
    }

    public async Task<Result<LocalizationBundleResponse>> GetLocalization(
        MetadataResponse metadataResponse,
        CancellationToken cancellationToken = default)
    {
        var request = new LocalizationBundleRequest
        {
            Id = metadataResponse.LatestLocalizationBundleVersion,
        };
        using var response = await _client.PostAsJsonAsync(
            "api/game/getlocalizationbundle",
            new
            {
                Payload = request
            },
            cancellationToken);
        return await response.Convert<LocalizationBundleResponse>(
            cancellationToken: cancellationToken);
    }

    public async Task<Result<MetadataResponse>> GetMetadata(
        CancellationToken cancellationToken = default)
    {
        var request = new MetadataRequest();
        using var response = await _client.PostAsJsonAsync(
            "api/content/getmetadata",
            new
            {
                Payload = request
            },
            cancellationToken);
        return await response.Convert<MetadataResponse>(cancellationToken: cancellationToken);
    }

    private static long GetGameDataItemSum()
        => GameDataItemsEnum.CategoryDefinitions +
            GameDataItemsEnum.SkillDefinitions +
            GameDataItemsEnum.EquipmentDefinitions +
            GameDataItemsEnum.AllTables +
            GameDataItemsEnum.BattleTargetingSets +
            GameDataItemsEnum.BattleTargetingRules +
            GameDataItemsEnum.AbilityDefinitions +
            GameDataItemsEnum.StatProgression +
            GameDataItemsEnum.StatMod +
            GameDataItemsEnum.ModRecommendations +
            GameDataItemsEnum.RelicTierDefinitions +
            GameDataItemsEnum.UnitDefinitions +
            GameDataItemsEnum.DatacronDefinitions;


}
