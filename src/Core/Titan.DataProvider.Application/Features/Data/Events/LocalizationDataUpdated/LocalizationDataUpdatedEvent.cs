using System;
using Titan.DataProvider.Domain.Abstractions;

namespace Titan.DataProvider.Application.Features.Data.Events.LocalizationDataUpdated
{
    public record LocalizationDataUpdatedEvent(Guid Id) : IDomainEvent
    {
    }
}
