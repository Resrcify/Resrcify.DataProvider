using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.SharedKernel.Web.Extensions;
using Resrcify.SharedKernel.Web.Primitives;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using Resrcify.DataProvider.Application.Features.Units.GetExpandedProfile;
using Resrcify.DataProvider.Application.Features.Units.GetExpandedProfiles;

namespace Resrcify.DataProvider.Presentation.Controllers;

[Route("api/[controller]")]
internal sealed class ProfileController(
    ISender sender)
    : ApiController(sender)
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