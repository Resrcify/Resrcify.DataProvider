using System;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.Entities
{
    public sealed class DatacronAbility : Entity
    {
        private DatacronAbility(Guid id) : base(id)
        {
        }
        public static Result<DatacronAbility> Create()
        {
            return new DatacronAbility(Guid.NewGuid());
        }
    }
}