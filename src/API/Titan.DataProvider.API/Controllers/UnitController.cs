using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Titan.DataProvider.API.Abstractions;
using Titan.DataProvider.API.Extensions;
using Titan.DataProvider.Application.Features.Units.Queries.GetExpandedUnitData;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.API.Controllers;

[Route("api/[controller]")]
public class UnitController : ApiController
{
    public UnitController(ISender sender) : base(sender)
    {
    }

    [HttpPost()]
    public async Task<IActionResult> GetExpandedUnitData([FromBody] PlayerProfileResponse playerProfile, CancellationToken cancellationToken = default)
        => await Result
            .Create(new GetExpandedUnitDataQuery(playerProfile, GetExpandedUnitDataQueryRequest.ENG_US))
            .Bind(request => _sender.Send(request, cancellationToken))
            .Match(Ok, HandleFailure);

    [HttpPost("{language}")]
    public async Task<IActionResult> GetExpandedUnitData([FromBody] PlayerProfileResponse playerProfile, GetExpandedUnitDataQueryRequest language, CancellationToken cancellationToken = default)
        => await Result
            .Create(new GetExpandedUnitDataQuery(playerProfile, language))
            .Bind(request => _sender.Send(request, cancellationToken))
            .Match(Ok, HandleFailure);
}