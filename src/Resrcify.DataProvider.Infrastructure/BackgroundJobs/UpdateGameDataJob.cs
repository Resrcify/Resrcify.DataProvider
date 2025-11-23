using System.Threading.Tasks;
using MediatR;
using Quartz;
using Resrcify.DataProvider.Application.Features.Data.UpdateRawData;
using Resrcify.DataProvider.Application.Features.Data.GetMetadataVersion;
using Resrcify.SharedKernel.Caching.Abstractions;
using System;
using Microsoft.Extensions.Logging;

namespace Resrcify.DataProvider.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
internal sealed class UpdateGameDataJob(
    ICachingService _cache,
    ISender _sender,
    ILogger<UpdateGameDataJob> _logger)
    : IJob
{
    private const string LocalKey = "LatestLocalizationBundleVersion";
    private const string GameDataKey = "LatestGameDataVersion";
    public async Task Execute(IJobExecutionContext context)
    {
        var metadata = await _sender.Send(
            new GetMetadataVersionQuery(),
            context.CancellationToken);

        if (metadata.IsFailure)
        {
            _logger.LogError("Failed to retrieve metadata: {Error}", metadata.Errors);
            return;
        }
        var latestGameDataVersion = metadata.Value?.LatestGamedataVersion?.Split(":")[1];
        var latestLocalizationBundleVersion = metadata.Value?.LatestLocalizationBundleVersion;

        var cachedLocalVersion = await _cache.GetAsync<string>(LocalKey, context.CancellationToken);
        var cachedGameDataVersion = await _cache.GetAsync<string>(GameDataKey, context.CancellationToken);

        if (latestGameDataVersion is null || latestLocalizationBundleVersion is null)
        {
            _logger.LogError("Failed to retrieve latestGameDataVersion or latestLocalizationBundleVersion");
            return;
        }

        if (cachedLocalVersion is not null && cachedGameDataVersion is not null &&
            latestGameDataVersion == cachedGameDataVersion &&
            latestLocalizationBundleVersion == cachedLocalVersion)
        {
            _logger.LogInformation("Metadata version equals cached version, skipping update");
            return;
        }

        var result = await _sender.Send(
            new UpdateRawDataCommand(),
            context.CancellationToken);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to update raw data: {Error}", result.Errors);
            return;
        }

        await _cache.SetAsync(
            LocalKey,
            latestLocalizationBundleVersion,
            TimeSpan.FromHours(24),
            context.CancellationToken);

        await _cache.SetAsync(
            GameDataKey,
            latestGameDataVersion,
            TimeSpan.FromHours(24),
            context.CancellationToken);

        if (await _cache.GetAsync<string>(LocalKey, context.CancellationToken) is null || await _cache.GetAsync<string>(GameDataKey, context.CancellationToken) is null)
        {
            _logger.LogError("Failed to save metadata versions to cache");
            return;
        }

        _logger.LogInformation("Saved metadata versions to cache");
    }
}