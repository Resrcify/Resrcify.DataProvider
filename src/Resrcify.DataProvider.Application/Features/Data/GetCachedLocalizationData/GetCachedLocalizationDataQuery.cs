using System.Collections.Generic;
using Resrcify.SharedKernel.Messaging.Abstractions;

namespace Resrcify.DataProvider.Application.Features.Data.GetCachedLocalizationData;

public sealed record GetCachedLocalizationDataQuery(
    GetCachedLocalizationDataQueryRequest Language)
    : IQuery<List<string>>;
