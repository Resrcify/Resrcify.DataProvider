using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Application.Errors;
using Titan.DataProvider.Application.Features.Data.Events.LocalizationDataUpdated;
using Titan.DataProvider.Application.Models.GalaxyOfHeroes.GameData;
using Titan.DataProvider.Application.Models.GalaxyOfHeroes.Localization;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Application.Features.Data.Commands.UpdateRawDataFromTitan
{
    public sealed class UpdateRawDataFromTitanCommandHandler : ICommandHandler<UpdateRawDataFromTitanCommand>
    {
        private readonly IGalaxyOfHeroesWrapperService _api;
        private readonly ICachingService _caching;
        private readonly IPublisher _publisher;

        public UpdateRawDataFromTitanCommandHandler(ICachingService caching, IGalaxyOfHeroesWrapperService api, IPublisher publisher)
        {
            _publisher = publisher;
            _api = api;
            _caching = caching;
        }

        public async Task<Result> Handle(UpdateRawDataFromTitanCommand request, CancellationToken cancellationToken)
        {
            var gameDataResponse = await _api.GetGameData();
            var localizationResponse = await _api.GetLocalization();

            if (!gameDataResponse.IsSuccessStatusCode || !localizationResponse.IsSuccessStatusCode)
                return Result.Failure(ApplicationErrors.HttpClient.RequestNotSuccessful);
            var gameData = JsonConvert.DeserializeObject<GameDataResponse>(await gameDataResponse.Content.ReadAsStringAsync(cancellationToken));
            var localization = JsonConvert.DeserializeObject<LocalizationBundleResponse>(await localizationResponse.Content.ReadAsStringAsync(cancellationToken));

            await _caching.SetAsync(nameof(GameDataResponse), gameData!, cancellationToken);
            await _caching.SetAsync(nameof(LocalizationBundleResponse), localization!, cancellationToken);

            await _publisher.Publish(new LocalizationDataUpdatedEvent(Guid.NewGuid()), cancellationToken);
            return Result.Success();
        }
    }
}