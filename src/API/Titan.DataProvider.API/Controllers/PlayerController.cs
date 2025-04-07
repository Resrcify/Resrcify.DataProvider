using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.SharedKernel.Web.Extensions;
using Resrcify.SharedKernel.Web.Primitives;
using Titan.DataProvider.Application.Features.Units.Queries.GetExpandedProfile;
using Titan.DataProvider.Application.Features.Units.Queries.GetExpandedProfiles;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

namespace Titan.DataProvider.API.Controllers;

[Route("api/[controller]")]
public class ProfileController(ISender sender) : ApiController(sender)
{
    [HttpPost()]
    public async Task<IResult> GetExpandedProfileData(
        [FromBody]
        PlayerProfileResponse playerProfile,
        string? definitionId = null,
        bool withStats = true,
        bool withoutGp = false,
        bool withoutModStats = false,
        bool withoutMods = false,
        bool withoutSkills = false,
        bool withoutDatacrons = false,
        CancellationToken cancellationToken = default)
        => await Result
            .Create(new GetExpandedProfileQuery(
                definitionId,
                playerProfile,
                GetExpandedProfileQueryRequest.ENG_US,
                withStats,
                withoutGp,
                withoutModStats,
                withoutMods,
                withoutSkills,
                withoutDatacrons))
            .Bind(request => Sender.Send(request, cancellationToken))
            .Match(Results.Ok, ToProblemDetails);

    [HttpPost("{language}")]
    public async Task<IResult> GetExpandedProfileData(
        [FromBody] PlayerProfileResponse playerProfile,
        GetExpandedProfileQueryRequest language,
        string? definitionId = null,
        bool withStats = true,
        bool withoutGp = false,
        bool withoutModStats = false,
        bool withoutMods = false,
        bool withoutSkills = false,
        bool withoutDatacrons = false,
        CancellationToken cancellationToken = default)
        => await Result
            .Create(new GetExpandedProfileQuery(
                definitionId,
                playerProfile,
                language,
                withStats,
                withoutGp,
                withoutModStats,
                withoutMods,
                withoutSkills,
                withoutDatacrons))
            .Bind(request => Sender.Send(request, cancellationToken))
            .Match(Results.Ok, ToProblemDetails);

    [RequestSizeLimit(int.MaxValue)]
    [HttpPost("/api/profiles")]
    public async Task<IResult> GetExpandedProfilesData(
        [FromBody]
        List<PlayerProfileResponse> playerProfiles,
        bool withStats = true,
        bool withoutGp = false,
        bool withoutModStats = false,
        bool withoutMods = false,
        bool withoutSkills = false,
        bool withoutDatacrons = false,
        CancellationToken cancellationToken = default)
        => await Result
            .Create(new GetExpandedProfilesQuery(
                playerProfiles,
                GetExpandedProfileQueryRequest.ENG_US,
                withStats,
                withoutGp,
                withoutModStats,
                withoutMods,
                withoutSkills,
                withoutDatacrons))
            .Bind(request => Sender.Send(request, cancellationToken))
            .Match(Results.Ok, ToProblemDetails);
}