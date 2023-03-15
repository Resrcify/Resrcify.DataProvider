using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Titan.DataProvider.API.Abstractions;
using Titan.DataProvider.API.Extensions;
using Titan.DataProvider.Application.Features.GameData.Commands;
using Titan.DataProvider.Domain.Shared;

namespace Titan.ShardManagement.API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ApiController
    {
        public UserController(ISender sender) : base(sender)
        {
        }
        [HttpPost("gamedata")]
        public async Task<IActionResult> UpdateGameData(CancellationToken cancellationToken = default)
            => await Result
                .Create(new UpdateGameDataCommand())
                .Bind(request => _sender.Send(request, cancellationToken))
                .Match(Ok, HandleFailure);
    }
}