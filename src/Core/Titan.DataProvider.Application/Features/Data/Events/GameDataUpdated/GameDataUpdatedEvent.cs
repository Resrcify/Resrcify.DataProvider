using System;
using Titan.DataProvider.Domain.Abstractions;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

namespace Titan.DataProvider.Application.Features.Data.Events.GameDataUpdated;

public record GameDataUpdatedEvent(Guid Id, GameDataResponse Data) : IDomainEvent
{
}
