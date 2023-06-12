
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Titan.DataProvider.Infrastructure.HttpClients;

public class ComlinkService : IGalaxyOfHeroesService
{
    public HttpClient Client { get; }

    public ComlinkService(HttpClient client)
    {
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        Client = client;
    }

    public async Task<HttpResponseMessage> GetGameData(string? version = null, CancellationToken cancellationToken = default)
    {
        var body = new StringContent(JsonConvert.SerializeObject(new
        {
            Payload = new
            {
                Version = version,
                IncludePveUnits = false,
                RequestSegment = 0
            },
            Enums = false
        }, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        }));
        body.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return await Client.PostAsync("data", body, cancellationToken);
    }

    public async Task<HttpResponseMessage> GetLocalization(string? version = null, CancellationToken cancellationToken = default)
    {
        var body = new StringContent(JsonConvert.SerializeObject(new
        {
            Payload = new
            {
                Id = version
            },
            Unzip = false
        }, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        }));
        body.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return await Client.PostAsync("localization", body, cancellationToken);
    }

    public async Task<HttpResponseMessage> GetMetadata(string? version = null, CancellationToken cancellationToken = default)
    {
        return await Client.PostAsync("metadata", null, cancellationToken);
    }
}
