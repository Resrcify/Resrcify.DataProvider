

using System;
using Titan.DataProvider.Domain.Abstractions;

namespace Titan.DataProvider.Domain.Primitives;

public abstract record DomainEvent(Guid Id) : IDomainEvent
{
}
