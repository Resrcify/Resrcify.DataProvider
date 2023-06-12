using MediatR;
using Titan.DataProvider.Domain.Abstractions;

namespace Titan.DataProvider.Application.Abstractions.Application.Messaging;

public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}
