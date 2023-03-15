using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.API.Abstractions
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected readonly ISender _sender;

        protected ApiController(ISender sender) => _sender = sender;

        protected IActionResult HandleFailure(Result result) =>
            result switch
            {
                { IsSuccess: true } => throw new InvalidOperationException(),
                _ =>
                    BadRequest(
                        CreateProblemDetails(
                            "Bad Request",
                            "Bad Request",
                            "One or more errors occurred",
                            StatusCodes.Status400BadRequest,
                            result.Errors))
            };

        private static ProblemDetails CreateProblemDetails(
            string title,
            string type,
            string detail,
            int status,
            Error[]? errors = null) =>
            new()
            {
                Title = title,
                Type = type,
                Detail = detail,
                Status = status,
                Extensions = { { nameof(errors), errors } }
            };
    }
}