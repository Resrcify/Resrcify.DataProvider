using System;
using System.Collections.Generic;
using System.Linq;
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
            var localization = await _caching.GetAsync<List<string>>($"Loc_ENG_US.txt", cancellationToken);
            if (localization is null) return;
            var data = BaseData.Create(notification.Data, localization);
            if (data.IsFailure) return;
            foreach (var item in data.Value.Units.Where(x => x.Value.CombatType == 2))
            {
                foreach (var item2 in item.Value.Crew)
                {
                    Console.WriteLine(item2);
                }
            }
            await _caching.SetAsync("BaseData", data.Value, cancellationToken);
            var secondTest = await _caching.GetAsync<BaseData>("BaseData", cancellationToken);
            Console.WriteLine("FETCHED");
            foreach (var item in secondTest!.Units.Where(x => x.Value.CombatType == 2))
            {
                foreach (var item2 in item.Value.Crew)
                {
                    Console.WriteLine(item2);
                }
            }
        }
    }
}