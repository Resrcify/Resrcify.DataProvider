using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.DataProvider.Domain.Internal.BaseData;

namespace Resrcify.DataProvider.Application.Features.Data.GetCachedBaseData;

public sealed record GetCachedBaseDataQuery(
    GetCachedBaseDataQueryRequest Language)
    : IQuery<BaseData>;
