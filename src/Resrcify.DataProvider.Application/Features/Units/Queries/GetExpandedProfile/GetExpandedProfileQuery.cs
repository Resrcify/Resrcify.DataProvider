using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

namespace Resrcify.DataProvider.Application.Features.Units.Queries.GetExpandedProfile;

public sealed record GetExpandedProfileQuery(
    string? DefinitionId,
    PlayerProfileResponse PlayerProfile,
    GetExpandedProfileQueryRequest Language,
    bool WithStats,
    bool WithoutGp,
    bool WithoutModStats,
    bool WithoutMods,
    bool WithoutSkills,
    bool WithoutDatacrons)
    : IQuery<GetExpandedProfileQueryResponse>;
