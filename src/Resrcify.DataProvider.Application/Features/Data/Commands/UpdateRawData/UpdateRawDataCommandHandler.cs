using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Resrcify.DataProvider.Application.Abstractions.Infrastructure;
using Resrcify.DataProvider.Application.Errors;
using Resrcify.DataProvider.Application.Features.Data.Events.LocalizationDataUpdated;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Resrcify.DataProvider.Application.Models.GalaxyOfHeroes.Localization;
using Resrcify.DataProvider.Application.Features.Data.Events.GameDataUpdated;
using Resrcify.DataProvider.Application.Models.GalaxyOfHeroes.Metadata;
using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Resrcify.DataProvider.Application.Features.Data.Commands.UpdateRawData;

public sealed class UpdateRawDataCommandHandler : ICommandHandler<UpdateRawDataCommand>
{
    private readonly IGalaxyOfHeroesService _api;
    private readonly IPublisher _publisher;

    public UpdateRawDataCommandHandler(IGalaxyOfHeroesService api, IPublisher publisher)
    {
        _publisher = publisher;
        _api = api;
    }

    public async Task<Result> Handle(UpdateRawDataCommand request, CancellationToken cancellationToken)
    {
        var metaDataResponse = await _api.GetMetadata(cancellationToken: cancellationToken);
        if (!metaDataResponse.IsSuccessStatusCode)
            return Result.Failure(ApplicationErrors.HttpClient.RequestNotSuccessful);
        var metaData = JsonConvert.DeserializeObject<MetadataResponse>(await metaDataResponse.Content.ReadAsStringAsync(cancellationToken));
        if (metaData is null)
            return Result.Failure(ApplicationErrors.HttpClient.RequestNotSuccessful);
        var gameDataResponse = await _api.GetGameData(version: metaData.LatestGamedataVersion, cancellationToken: cancellationToken);
        var localizationResponse = await _api.GetLocalization(version: metaData.LatestLocalizationBundleVersion, cancellationToken: cancellationToken);

        if (!gameDataResponse.IsSuccessStatusCode || !localizationResponse.IsSuccessStatusCode)
            return Result.Failure(ApplicationErrors.HttpClient.RequestNotSuccessful);
        var gameData = JsonConvert.DeserializeObject<GameDataResponse>(await gameDataResponse.Content.ReadAsStringAsync(cancellationToken));
        var localization = JsonConvert.DeserializeObject<LocalizationBundleResponse>(await localizationResponse.Content.ReadAsStringAsync(cancellationToken));

        await _publisher.Publish(new LocalizationDataUpdatedEvent(Guid.NewGuid(), localization!), cancellationToken);
        await _publisher.Publish(new GameDataUpdatedEvent(Guid.NewGuid(), gameData!), cancellationToken);
        return Result.Success();
    }
}
