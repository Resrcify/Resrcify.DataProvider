using System.Collections.Generic;
using Resrcify.SharedKernel.Messaging.Abstractions;

namespace Titan.DataProvider.Application.Features.Data.Queries.GetCachedLocalizationData;

public sealed record GetCachedLocalizationDataQuery(
    GetCachedLocalizationDataQueryRequest Language)
    : IQuery<List<string>>;
