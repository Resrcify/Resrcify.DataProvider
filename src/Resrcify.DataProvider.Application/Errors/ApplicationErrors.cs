using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Resrcify.DataProvider.Application.Errors;

public static class ApplicationErrors
{
    public static class HttpClient
    {
        public static readonly Error RequestNotSuccessful = new(
            "HttpClient.RequestNotSuccessful",
            $"The request performed by the HttpClient was not successful.",
            ErrorType.Failure);
    }
}
