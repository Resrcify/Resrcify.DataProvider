using System.Threading;
using System.Threading.Tasks;
using Resrcify.DataProvider.Application.Models;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Resrcify.DataProvider.Application.Abstractions;

public interface IGalaxyOfHeroesService
{
    Task<Result<GameDataResponse>> GetGameData(CancellationToken cancellationToken = default);
    Task<Result<LocalizationBundleResponse>> GetLocalization(CancellationToken cancellationToken = default);
    Task<Result<MetadataResponse>> GetMetadata(CancellationToken cancellationToken = default);
}
