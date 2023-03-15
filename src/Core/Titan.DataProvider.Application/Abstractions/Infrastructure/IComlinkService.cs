using System.Net.Http;
using System.Threading.Tasks;

namespace Titan.DataProvider.Application.Abstractions.Infrastructure
{
    public interface IComlinkService
    {
        Task<HttpResponseMessage> GetGameData();
        Task<HttpResponseMessage> GetLocalization();
    }
}