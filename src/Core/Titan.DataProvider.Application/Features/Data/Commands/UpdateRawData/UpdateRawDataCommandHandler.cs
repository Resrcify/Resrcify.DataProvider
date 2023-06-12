using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Application.Errors;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Titan.DataProvider.Application.Models.GalaxyOfHeroes.Localization;
using Titan.DataProvider.Application.Models.GalaxyOfHeroes.Metadata;
using Titan.DataProvider.Domain.Shared;
using MediatR;
using Titan.DataProvider.Application.Features.Data.Events.LocalizationDataUpdated;
using System;

namespace Titan.DataProvider.Application.Features.Data.Commands.UpdateRawData;

public sealed class UpdateRawDataCommandHandler : ICommandHandler<UpdateRawDataCommand>
{
    private readonly IComlinkService _api;
    private readonly ICachingService _caching;
    private readonly IPublisher _publisher;
    public UpdateRawDataCommandHandler(ICachingService caching, IComlinkService api, IPublisher publisher)
    {
        _api = api;
        _caching = caching;
        _publisher = publisher;
    }

    public async Task<Result> Handle(UpdateRawDataCommand request, CancellationToken cancellationToken)
    {
        var metadataResponse = await _api.GetMetadata();
        if (!metadataResponse.IsSuccessStatusCode)
            return Result.Failure(ApplicationErrors.HttpClient.RequestNotSuccessful);

        var metadata = JsonConvert.DeserializeObject<MetadataResponse>(await metadataResponse.Content.ReadAsStringAsync(cancellationToken));

        var gameDataResponse = await _api.GetGameData(metadata!.LatestGamedataVersion!);
        var localizationResponse = await _api.GetLocalization(metadata.LatestLocalizationBundleVersion!);

        if (!gameDataResponse.IsSuccessStatusCode || !localizationResponse.IsSuccessStatusCode)
            return Result.Failure(ApplicationErrors.HttpClient.RequestNotSuccessful);
        var gameData = JsonConvert.DeserializeObject<GameDataResponse>(await gameDataResponse.Content.ReadAsStringAsync(cancellationToken));
        var localization = JsonConvert.DeserializeObject<LocalizationBundleResponse>(await localizationResponse.Content.ReadAsStringAsync(cancellationToken));

        await _caching.SetAsync("GameData", gameData!, cancellationToken);

        await _publisher.Publish(new LocalizationDataUpdatedEvent(Guid.NewGuid(), localization!), cancellationToken);
        return Result.Success();
    }
}
