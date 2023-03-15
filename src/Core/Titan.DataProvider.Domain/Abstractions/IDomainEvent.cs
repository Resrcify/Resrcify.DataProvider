using System;
using MediatR;

namespace Titan.DataProvider.Domain.Abstractions
{
    public interface IDomainEvent : INotification
    {
        public Guid Id { get; init; }
    }
}
