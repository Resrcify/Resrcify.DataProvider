using System.Collections.Generic;
using MediatR;
using Titan.DataProvider.Application.Features.Units.Queries.GetExpandedProfile;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

namespace Titan.DataProvider.Application.Features.Units.Queries.GetExpandedProfiles;

public sealed record GetExpandedProfilesQuery(
    List<PlayerProfileResponse> PlayerProfiles,
    GetExpandedProfileQueryRequest Language,
    bool WithStats,
    bool WithoutGp,
    bool WithoutModStats,
    bool WithoutMods,
    bool WithoutSkills,
    bool WithoutDatacrons) : IRequest<IEnumerable<GetExpandedProfilesQueryResponse>>
{
}
