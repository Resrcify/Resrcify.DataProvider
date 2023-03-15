using System;

namespace Titan.DataProvider.Domain.Abstractions
{
    public interface IAuditableEntity
    {
        DateTime CreatedOnUtc { get; set; }
        DateTime ModifiedOnUtc { get; set; }
    }
}