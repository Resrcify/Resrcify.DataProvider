using System.Collections.Generic;
using Resrcify.DataProvider.Application.Features.Units.Common;
using Resrcify.DataProvider.Domain.Internal.ExpandedDatacron;
using Resrcify.DataProvider.Domain.Internal.ExpandedUnit;

namespace Resrcify.DataProvider.Application.Features.Units.GetExpandedProfile;

public sealed record GetExpandedProfileQueryResponse(
    string PlayerId,
    long AllyCode,
    string Name,
    ProfileSummary ProfileSummary,
    Dictionary<string, ExpandedUnit> Units,
    IEnumerable<ExpandedDatacron> Datacrons);