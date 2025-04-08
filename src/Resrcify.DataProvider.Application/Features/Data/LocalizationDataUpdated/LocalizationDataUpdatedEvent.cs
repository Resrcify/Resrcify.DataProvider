using System;
using Resrcify.SharedKernel.DomainDrivenDesign.Abstractions;
using Resrcify.DataProvider.Application.Models.GalaxyOfHeroes.Localization;

namespace Resrcify.DataProvider.Application.Features.Data.LocalizationDataUpdated;

public record LocalizationDataUpdatedEvent(
    Guid Id,
    LocalizationBundleResponse Localization)
    : IDomainEvent;
