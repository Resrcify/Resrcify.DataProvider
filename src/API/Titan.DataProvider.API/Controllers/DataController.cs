using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Titan.DataProvider.API.Abstractions;
using Titan.DataProvider.API.Extensions;
using Titan.DataProvider.Application.Features.Data.Commands.UpdateRawData;
using Titan.DataProvider.Application.Features.Data.Commands.UpdateRawDataFromTitan;
using Titan.DataProvider.Application.Features.Data.Queries.GetCachedBaseData;
using Titan.DataProvider.Application.Features.Data.Queries.GetCachedLocalizationData;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.API.Controllers
{
    [Route("api/[controller]")]
    public class DataController : ApiController
    {
        public DataController(ISender sender) : base(sender)
        {
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateRawData(CancellationToken cancellationToken = default)
            => await Result
                .Create(new UpdateRawDataCommand())
                .Bind(request => _sender.Send(request, cancellationToken))
                .Match(Ok, HandleFailure);

        [HttpPost("titan/update")]
        public async Task<IActionResult> UpdateRawDataFromTitan(CancellationToken cancellationToken = default)
            => await Result
                .Create(new UpdateRawDataFromTitanCommand())
                .Bind(request => _sender.Send(request, cancellationToken))
                .Match(Ok, HandleFailure);

        [HttpGet("localization/{language}")]
        public async Task<IActionResult> GetCachedLocalization([FromRoute] GetCachedLocalizationDataQueryRequest language, CancellationToken cancellationToken = default)
            => await Result
                .Create(new GetCachedLocalizationDataQuery(language))
                .Bind(request => _sender.Send(request, cancellationToken))
                .Match(Ok, HandleFailure);

        [HttpGet("base/{language}")]
        public async Task<IActionResult> GetCachedBaseData([FromRoute] GetCachedBaseDataQueryRequest language, CancellationToken cancellationToken = default)
            => await Result
                .Create(new GetCachedBaseDataQuery(language))
                .Bind(request => _sender.Send(request, cancellationToken))
                .Match(Ok, HandleFailure);
    }
}