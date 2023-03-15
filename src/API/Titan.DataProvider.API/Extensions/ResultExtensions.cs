using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.API.Extensions
{
    internal static class ResultExtensions
    {
        internal static async Task<IActionResult> Match(
            this Task<Result> resultTask,
            Func<IActionResult> onSuccess,
            Func<Result, IActionResult> onFailure
        )
        {
            var result = await resultTask;
            return result.IsSuccess ? onSuccess() : onFailure(result);
        }

        internal static async Task<IActionResult> Match<TIn>(
            this Task<Result<TIn>> resultTask,
            Func<TIn, IActionResult> onSuccess,
            Func<Result, IActionResult> onFailure
        )
        {
            var result = await resultTask;
            return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
        }
    }
}