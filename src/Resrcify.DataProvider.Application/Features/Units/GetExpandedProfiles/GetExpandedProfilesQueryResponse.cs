using System.Collections.Generic;
using Resrcify.DataProvider.Application.Features.Units.Common;
using Resrcify.DataProvider.Domain.Internal.ExpandedDatacron;
using Resrcify.DataProvider.Domain.Internal.ExpandedUnit;

namespace Resrcify.DataProvider.Application.Features.Units.GetExpandedProfiles;

public sealed record GetExpandedProfilesQueryResponse(
    string PlayerId,
    long AllyCode,
    string Name,
    ProfileSummary ProfileSummary,
    Dictionary<string, ExpandedUnit> Units,
    IEnumerable<ExpandedDatacron> Datacrons);