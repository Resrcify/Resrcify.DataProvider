using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Application.Errors
{
    public static class ApplicationErrors
    {

        public static class HttpClient
        {
            public static readonly Error RequestNotSuccessful = new(
                "HttpClient.RequestNotSuccessful",
                $"The request performed by the HttpClient was not successful.");
        }
    }
}
