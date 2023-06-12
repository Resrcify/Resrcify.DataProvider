using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Domain.Internal.BaseData;

namespace Titan.DataProvider.Application.Features.Data.Events.GameDataUpdated;

public sealed class GameDataUpdatedEventHandler : IDomainEventHandler<GameDataUpdatedEvent>
{
    private readonly ICachingService _caching;

    public GameDataUpdatedEventHandler(ICachingService caching)
       => _caching = caching;

    public async Task Handle(GameDataUpdatedEvent notification, CancellationToken cancellationToken)
    {
        if (notification?.Data is null) return;
        foreach (var local in Enum.GetNames(typeof(LocalizationType)))
        {
            var localization = await _caching.GetAsync<List<string>>($"Loc_{local}.txt", cancellationToken);
            if (localization is null) return;
            var data = BaseData.Create(notification.Data, localization);
            if (data.IsFailure) return;
            await _caching.SetAsync($"BaseData-{local}", data.Value, cancellationToken);
        }
    }
}
