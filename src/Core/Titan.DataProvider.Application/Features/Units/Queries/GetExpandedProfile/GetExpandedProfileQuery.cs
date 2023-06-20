using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

namespace Titan.DataProvider.Application.Features.Units.Queries.GetExpandedProfile;

public sealed record GetExpandedProfileQuery(
    string? DefinitionId,
    PlayerProfileResponse PlayerProfile,
    GetExpandedProfileQueryRequest Language,
    bool WithStats,
    bool WithoutGp,
    bool WithoutMods,
    bool WithoutSkills,
    bool WithoutDatacrons) : IQuery<GetExpandedProfileQueryResponse>
{
}
