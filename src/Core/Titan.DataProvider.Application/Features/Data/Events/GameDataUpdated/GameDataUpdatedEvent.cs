using System;
using Resrcify.SharedKernel.DomainDrivenDesign.Abstractions;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

namespace Titan.DataProvider.Application.Features.Data.Events.GameDataUpdated;

public record GameDataUpdatedEvent(
    Guid Id,
    GameDataResponse Data)
    : IDomainEvent;
