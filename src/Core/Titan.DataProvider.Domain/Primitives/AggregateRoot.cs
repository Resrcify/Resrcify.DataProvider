using System;
using System.Collections.Generic;
using System.Linq;
using Titan.DataProvider.Domain.Abstractions;

namespace Titan.DataProvider.Domain.Primitives
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IDomainEvent> _domainEvents = new();
        protected AggregateRoot(Guid id) : base(id)
        {
        }

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
            _domainEvents.Add(domainEvent);
    }
}
