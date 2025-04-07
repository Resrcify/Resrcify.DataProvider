using System;
using Resrcify.SharedKernel.DomainDrivenDesign.Abstractions;
using Titan.DataProvider.Application.Models.GalaxyOfHeroes.Localization;

namespace Titan.DataProvider.Application.Features.Data.Events.LocalizationDataUpdated;

public record LocalizationDataUpdatedEvent(
    Guid Id,
    LocalizationBundleResponse Localization)
    : IDomainEvent;
