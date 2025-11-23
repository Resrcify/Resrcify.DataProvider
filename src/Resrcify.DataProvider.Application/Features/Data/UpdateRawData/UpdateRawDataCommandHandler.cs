using System;
using System.Threading;
using System.Threading.Tasks;
using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.DataProvider.Application.Abstractions;
using System.IO.Compression;
using System.IO;
using System.Linq;
using Resrcify.SharedKernel.Caching.Abstractions;
using System.Collections.Generic;
using System.Text;
using Resrcify.DataProvider.Domain.Internal.BaseData;
using Resrcify.DataProvider.Application.Features.Data.GetCachedLocalizationData;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Resrcify.DataProvider.Application.Extensions;

namespace Resrcify.DataProvider.Application.Features.Data.UpdateRawData;

internal sealed class UpdateRawDataCommandHandler(
    ISwgohApiService _api,
    ICachingService _caching)
    : ICommandHandler<UpdateRawDataCommand>
{
    public async Task<Result> Handle(UpdateRawDataCommand request, CancellationToken cancellationToken)
    {
        var metaDataResponse = await _api.GetMetadata(
            cancellationToken: cancellationToken);
        if (metaDataResponse.IsFailure)
            return metaDataResponse;

        var gameDataResponse = await _api.GetGameData(
            metaDataResponse.Value,
            cancellationToken: cancellationToken);

        if (gameDataResponse.IsFailure)
            return gameDataResponse;

        var localizationResponse = await _api.GetLocalization(
            metaDataResponse.Value,
            cancellationToken: cancellationToken);

        if (localizationResponse.IsFailure)
            return localizationResponse;

        var localDictionary = CreateLocalizationDictionary(localizationResponse.Value.LocalizationBundle);
        var baseDataDictionary = CreateBaseDataDictionary(gameDataResponse.Value, localDictionary);

        await CacheLocalization(
            localDictionary,
            cancellationToken);

        await CacheBaseData(
            baseDataDictionary,
            cancellationToken);

        var errors = new List<Error>();
        foreach (var key in localDictionary.Keys)
        {
            var local = await _caching.GetAsync<List<string>>(
                $"{key}",
                null,
                cancellationToken);
            if (local is null)
            {
                errors.Add(
                    Error.Failure(
                        "LocalizationCache.Empty",
                        $"Failed to cache localization file: {key}"));
            }
        }
        foreach (var key in baseDataDictionary.Keys)
        {
            var baseData = await _caching.GetAsync<BaseData>(
                $"BaseData-{key}",
                JsonSerializerExtensions.GetDomainSerializerOptions(),
                cancellationToken);
            if (baseData is null)
            {
                errors.Add(
                    Error.Failure(
                        "BaseDataCache.Empty",
                        $"Failed to cache base data for locale: {key}"));
            }
        }
        if (errors.Count > 0)
            return Result.Failure([.. errors]);
        return Result.Success();
    }

    private async Task CacheBaseData(
        Dictionary<string, Result<BaseData>> baseDataDictionary,
        CancellationToken cancellationToken)
    {
        var tasks = baseDataDictionary.Select(kvp => _caching.SetAsync(
            $"BaseData-{kvp.Key}",
            kvp.Value.Value,
            TimeSpan.MaxValue,
            JsonSerializerExtensions.GetDomainSerializerOptions(),
            cancellationToken));

        await Task.WhenAll(tasks);
    }

    private static Dictionary<string, Result<BaseData>> CreateBaseDataDictionary(
        GameDataResponse gameDataResponse,
        Dictionary<string, List<string>> localDictionary)
        => Enum
            .GetNames<GetCachedLocalizationDataQueryRequest>()
            .Where(localName => localDictionary.TryGetValue($"Loc_{localName}.txt", out _))
            .Select(localName => new
            {
                Key = localName,
                Value = BaseData.Create(
                    gameDataResponse,
                    localDictionary[$"Loc_{localName}.txt"])
            })
            .Where(item => item.Value.IsSuccess)
            .ToDictionary(
                item => item.Key,
                item => item.Value);

    private static Dictionary<string, List<string>> CreateLocalizationDictionary(byte[]? localization)
    {
        if (localization is null)
            return [];
        using var memoryStream = new MemoryStream(localization);
        using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read);
        return archive.Entries
            .Select(entry => entry)
            .ToDictionary(
                entry => entry.Name,
                entry => GetContents(entry).ToList());
    }
    private async Task CacheLocalization(
        Dictionary<string, List<string>> localDictionary,
        CancellationToken cancellationToken = default)
    {
        var tasks = localDictionary.Select(kvp => _caching.SetAsync(
            kvp.Key,
            kvp.Value,
            TimeSpan.MaxValue,
            null,
            cancellationToken));

        await Task.WhenAll(tasks);
    }

    private static IEnumerable<string> GetContents(ZipArchiveEntry e)
    {
        using StreamReader stm = new(e.Open(), Encoding.UTF8);
        if (stm == null)
            yield break;
        string line;
        while ((line = stm.ReadLine()!) != null)
            yield return line;
    }
}