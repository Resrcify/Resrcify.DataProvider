using System.Threading;
using System.Threading.Tasks;
using Resrcify.DataProvider.Application.Models;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Resrcify.DataProvider.Application.Abstractions;

public interface ISwgohApiService
{
    Task<Result<GameDataResponse>> GetGameData(
        MetadataResponse metadataResponse,
        CancellationToken cancellationToken = default);
    Task<Result<LocalizationBundleResponse>> GetLocalization(
        MetadataResponse metadataResponse,
        CancellationToken cancellationToken = default);
    Task<Result<MetadataResponse>> GetMetadata(
        CancellationToken cancellationToken = default);
}
