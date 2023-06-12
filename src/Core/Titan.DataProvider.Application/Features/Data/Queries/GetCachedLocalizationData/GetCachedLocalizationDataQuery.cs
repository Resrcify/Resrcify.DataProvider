using System.Collections.Generic;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;

namespace Titan.DataProvider.Application.Features.Data.Queries.GetCachedLocalizationData;

public sealed record GetCachedLocalizationDataQuery(GetCachedLocalizationDataQueryRequest Language) : IQuery<List<string>>
{
}
