
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Infrastructure;

namespace Titan.DataProvider.Infrastructure.HttpClients;

public class ComlinkService : IComlinkService
{
    public HttpClient Client { get; }

    public ComlinkService(HttpClient client)
    {
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        Client = client;
    }

    public Task<HttpResponseMessage> GetGameData(string gamedataVersion)
    {
        throw new System.NotImplementedException();
    }

    public Task<HttpResponseMessage> GetLocalization(string localizationBundleVersion)
    {
        throw new System.NotImplementedException();
    }

    public Task<HttpResponseMessage> GetMetadata()
    {
        throw new System.NotImplementedException();
    }
}
