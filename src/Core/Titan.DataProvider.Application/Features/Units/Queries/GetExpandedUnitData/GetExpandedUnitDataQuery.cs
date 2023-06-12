using System.Collections.Generic;
using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Domain.Internal.ExpandedUnit;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

namespace Titan.DataProvider.Application.Features.Units.Queries.GetExpandedUnitData;

public sealed record GetExpandedUnitDataQuery(PlayerProfileResponse PlayerProfile, GetExpandedUnitDataQueryRequest Language) : IQuery<List<ExpandedUnit>>
{
}
