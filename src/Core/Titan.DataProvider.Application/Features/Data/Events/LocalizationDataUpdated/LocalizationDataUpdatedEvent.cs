using System;
using Titan.DataProvider.Application.Models.GalaxyOfHeroes.Localization;
using Titan.DataProvider.Domain.Abstractions;

namespace Titan.DataProvider.Application.Features.Data.Events.LocalizationDataUpdated;

public record LocalizationDataUpdatedEvent(Guid Id, LocalizationBundleResponse Localization) : IDomainEvent
{
}
