using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Domain.Internal.BaseData;

namespace Titan.DataProvider.Application.Features.Data.Events.GameDataUpdated
{
    public sealed class GameDataUpdatedEventHandler : IDomainEventHandler<GameDataUpdatedEvent>
    {
        private readonly ICachingService _caching;

        public GameDataUpdatedEventHandler(ICachingService caching)
        {
            _caching = caching;
        }

        public async Task Handle(GameDataUpdatedEvent notification, CancellationToken cancellationToken)
        {
            if (notification?.Data is null) return;
            var data = BaseData.Create(notification?.Data!);
            if (data.IsFailure) return;
            // Console.WriteLine(JsonConvert.SerializeObject(data));
            await _caching.SetAsync("BaseData", data.Value, cancellationToken);
        }
    }
}