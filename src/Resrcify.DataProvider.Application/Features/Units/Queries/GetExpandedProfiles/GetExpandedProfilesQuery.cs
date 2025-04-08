using System.Collections.Generic;
using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.DataProvider.Application.Features.Units.Queries.GetExpandedProfile;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

namespace Resrcify.DataProvider.Application.Features.Units.Queries.GetExpandedProfiles;

public sealed record GetExpandedProfilesQuery(
    List<PlayerProfileResponse> PlayerProfiles,
    GetExpandedProfileQueryRequest Language,
    bool WithStats,
    bool WithoutGp,
    bool WithoutModStats,
    bool WithoutMods,
    bool WithoutSkills,
    bool WithoutDatacrons)
    : IQuery<IEnumerable<GetExpandedProfilesQueryResponse>>;
