using Resrcify.SharedKernel.Messaging.Abstractions;
using Titan.DataProvider.Domain.Internal.BaseData;

namespace Titan.DataProvider.Application.Features.Data.Queries.GetCachedBaseData;

public sealed record GetCachedBaseDataQuery(
    GetCachedBaseDataQueryRequest Language)
    : IQuery<BaseData>;
