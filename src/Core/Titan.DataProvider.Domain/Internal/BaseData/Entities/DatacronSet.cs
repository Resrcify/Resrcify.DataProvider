using System;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.Entities
{
    public sealed class DatacronSet : Entity
    {
        private DatacronSet(Guid id) : base(id)
        {
        }
        public static Result<DatacronSet> Create()
        {
            return new DatacronSet(Guid.NewGuid());
        }
    }
}