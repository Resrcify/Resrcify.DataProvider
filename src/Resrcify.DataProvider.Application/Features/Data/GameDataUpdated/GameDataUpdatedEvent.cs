using System;
using Resrcify.SharedKernel.DomainDrivenDesign.Abstractions;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

namespace Resrcify.DataProvider.Application.Features.Data.GameDataUpdated;

public record GameDataUpdatedEvent(
    Guid Id,
    GameDataResponse Data)
    : IDomainEvent;
