using System;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.Entities
{
    public sealed class Table : Entity
    {
        private Table(Guid id) : base(id)
        {
        }
        public static Result<Table> Create()
        {
            return new Table(Guid.NewGuid());
        }
    }
}