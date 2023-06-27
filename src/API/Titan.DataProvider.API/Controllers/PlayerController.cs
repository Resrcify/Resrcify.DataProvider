using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Titan.DataProvider.API.Abstractions;
using Titan.DataProvider.API.Extensions;
using Titan.DataProvider.Application.Features.Units.Queries.GetExpandedProfile;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.API.Controllers;

[Route("api/[controller]")]
public class ProfileController : ApiController
{
    public ProfileController(ISender sender) : base(sender)
    {
    }

    [HttpPost()]
    public async Task<IActionResult> GetExpandedProfileData(
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
            .Create(new GetExpandedProfileQuery(definitionId, playerProfile, GetExpandedProfileQueryRequest.ENG_US, withStats, withoutGp, withoutModStats, withoutMods, withoutSkills, withoutDatacrons))
            .Bind(request => _sender.Send(request, cancellationToken))
            .Match(Ok, HandleFailure);

    [HttpPost("{language}")]
    public async Task<IActionResult> GetExpandedProfileData(
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
            .Create(new GetExpandedProfileQuery(definitionId, playerProfile, language, withStats, withoutGp, withoutModStats, withoutMods, withoutSkills, withoutDatacrons))
            .Bind(request => _sender.Send(request, cancellationToken))
            .Match(Ok, HandleFailure);
}