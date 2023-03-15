
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Infrastructure;

namespace Titan.ShardManagement.Infrastructure.GalaxyOfHeroesWrapper
{
    public class ComlinkService : IComlinkService
    {
        public HttpClient Client { get; }

        public ComlinkService(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client = client;
        }

        public Task<HttpResponseMessage> GetGameData()
        {
            throw new System.NotImplementedException();
        }

        public Task<HttpResponseMessage> GetLocalization()
        {
            throw new System.NotImplementedException();
        }
    }
}
