using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Domain.Internal.BaseData;

namespace Titan.DataProvider.Application.Features.Data.Queries.GetCachedBaseData
{
    public sealed record GetCachedBaseDataQuery() : IQuery<BaseData>
    {
    }
}