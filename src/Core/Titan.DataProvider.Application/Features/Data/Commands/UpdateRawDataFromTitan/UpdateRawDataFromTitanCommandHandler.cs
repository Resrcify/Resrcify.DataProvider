using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Application.Errors;
using Titan.DataProvider.Application.Features.Data.Events.LocalizationDataUpdated;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Titan.DataProvider.Application.Models.GalaxyOfHeroes.Localization;
using Titan.DataProvider.Domain.Shared;
using Titan.DataProvider.Application.Features.Data.Events.GameDataUpdated;

namespace Titan.DataProvider.Application.Features.Data.Commands.UpdateRawDataFromTitan
{
    public sealed class UpdateRawDataFromTitanCommandHandler : ICommandHandler<UpdateRawDataFromTitanCommand>
    {
        private readonly IGalaxyOfHeroesWrapperService _api;
        private readonly IPublisher _publisher;

        public UpdateRawDataFromTitanCommandHandler(IGalaxyOfHeroesWrapperService api, IPublisher publisher)
        {
            _publisher = publisher;
            _api = api;
        }

        public async Task<Result> Handle(UpdateRawDataFromTitanCommand request, CancellationToken cancellationToken)
        {
            var gameDataResponse = await _api.GetGameData();
            var localizationResponse = await _api.GetLocalization();

            if (!gameDataResponse.IsSuccessStatusCode || !localizationResponse.IsSuccessStatusCode)
                return Result.Failure(ApplicationErrors.HttpClient.RequestNotSuccessful);
            var gameData = JsonConvert.DeserializeObject<GameDataResponse>(await gameDataResponse.Content.ReadAsStringAsync(cancellationToken));
            var localization = JsonConvert.DeserializeObject<LocalizationBundleResponse>(await localizationResponse.Content.ReadAsStringAsync(cancellationToken));

            await _publisher.Publish(new LocalizationDataUpdatedEvent(Guid.NewGuid(), localization!), cancellationToken);
            await _publisher.Publish(new GameDataUpdatedEvent(Guid.NewGuid(), gameData!), cancellationToken);
            return Result.Success();
        }
    }
}