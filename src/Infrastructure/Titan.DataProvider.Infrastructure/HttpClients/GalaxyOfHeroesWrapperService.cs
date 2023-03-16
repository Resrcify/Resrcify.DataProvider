
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Infrastructure;

namespace Titan.DataProvider.Infrastructure.HttpClients
{
    public class GalaxyOfHeroesWrapperService : IGalaxyOfHeroesWrapperService
    {
        public HttpClient Client { get; }

        public GalaxyOfHeroesWrapperService(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client = client;
        }

        public async Task<HttpResponseMessage> GetGameData()
        {
            return await Client.GetAsync("api/data/getgamedata");
        }

        public async Task<HttpResponseMessage> GetLocalization()
        {
            return await Client.GetAsync("api/data/getlocalizationdata");
        }
    }
}
