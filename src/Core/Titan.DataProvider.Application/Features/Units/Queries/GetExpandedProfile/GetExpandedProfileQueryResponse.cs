using System.Collections.Generic;
using Titan.DataProvider.Domain.Internal.ExpandedDatacron;
using Titan.DataProvider.Domain.Internal.ExpandedUnit;

namespace Titan.DataProvider.Application.Features.Units.Queries.GetExpandedProfile;

public sealed record GetExpandedProfileQueryResponse(Dictionary<string, ExpandedUnit> Units, IEnumerable<ExpandedDatacron> Datacrons)
{
}