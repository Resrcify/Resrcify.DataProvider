using System;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.Entities
{
    public sealed class Relic : Entity
    {
        private Relic(Guid id) : base(id)
        {
        }
        public static Result<Relic> Create()
        {
            return new Relic(Guid.NewGuid());
        }
    }
}