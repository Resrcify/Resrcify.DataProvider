using System.Net.Http;
using System.Threading.Tasks;

namespace Titan.DataProvider.Application.Abstractions.Infrastructure
{
    public interface IComlinkService
    {
        Task<HttpResponseMessage> GetGameData(string gamedataVersion);
        Task<HttpResponseMessage> GetLocalization(string localizationBundleVersion);
        Task<HttpResponseMessage> GetMetadata();
    }
}