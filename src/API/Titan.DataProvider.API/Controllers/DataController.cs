using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.SharedKernel.Web.Extensions;
using Resrcify.SharedKernel.Web.Primitives;
using Titan.DataProvider.Application.Features.Data.Commands.UpdateRawData;
using Titan.DataProvider.Application.Features.Data.Queries.GetCachedBaseData;
using Titan.DataProvider.Application.Features.Data.Queries.GetCachedLocalizationData;

namespace Titan.DataProvider.API.Controllers;

[Route("api/[controller]")]
public class DataController(ISender sender) : ApiController(sender)
{
    [HttpPost("update")]
    public async Task<IResult> UpdateRawData(CancellationToken cancellationToken = default)
        => await Result
            .Create(new UpdateRawDataCommand())
            .Bind(request => Sender.Send(request, cancellationToken))
            .Match(Results.NoContent, ToProblemDetails);

    [HttpGet("localization/{language}")]
    public async Task<IResult> GetCachedLocalization([FromRoute] GetCachedLocalizationDataQueryRequest language, CancellationToken cancellationToken = default)
        => await Result
            .Create(new GetCachedLocalizationDataQuery(language))
            .Bind(request => Sender.Send(request, cancellationToken))
            .Match(Results.Ok, ToProblemDetails);
    [HttpGet("localization")]
    public async Task<IResult> GetCachedLocalization(CancellationToken cancellationToken = default)
        => await Result
            .Create(new GetCachedLocalizationDataQuery(GetCachedLocalizationDataQueryRequest.ENG_US))
            .Bind(request => Sender.Send(request, cancellationToken))
            .Match(Results.Ok, ToProblemDetails);

    [HttpGet("base/{language}")]
    public async Task<IResult> GetCachedBaseData([FromRoute] GetCachedBaseDataQueryRequest language, CancellationToken cancellationToken = default)
        => await Result
            .Create(new GetCachedBaseDataQuery(language))
            .Bind(request => Sender.Send(request, cancellationToken))
            .Match(Results.Ok, ToProblemDetails);
    [HttpGet("base")]
    public async Task<IResult> GetCachedBaseData(CancellationToken cancellationToken = default)
        => await Result
            .Create(new GetCachedBaseDataQuery(GetCachedBaseDataQueryRequest.ENG_US))
            .Bind(request => Sender.Send(request, cancellationToken))
            .Match(Results.Ok, ToProblemDetails);
}