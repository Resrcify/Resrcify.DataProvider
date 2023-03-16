using System;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.Entities
{
    public sealed class Unit : Entity
    {
        private Unit(Guid id) : base(id)
        {
        }
        public static Result<Unit> Create()
        {
            return new Unit(Guid.NewGuid());
        }
    }
}