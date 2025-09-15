using System.Threading.Tasks;
using MediatR;
using Quartz;
using Resrcify.DataProvider.Application.Features.Data.UpdateRawData;
using Resrcify.DataProvider.Application.Features.Data.GetMetadataVersion;
using Resrcify.SharedKernel.Caching.Abstractions;
using System;

namespace Resrcify.DataProvider.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
internal sealed class UpdateGameDataJob(
    ICachingService _cache,
    ISender _sender)
    : IJob
{
    private const string CachedLocalKey = "LatestLocalizationBundleVersion";
    private const string CachedGameDataKey = "LatestGameDataVersion";
    public async Task Execute(IJobExecutionContext context)
    {
        var metadata = await _sender.Send(new GetMetadataVersionQuery(), context.CancellationToken);
        if (metadata.IsFailure)
            return;
        var latestGameDataVersion = metadata.Value?.LatestGamedataVersion?.Split(":")[1];
        var latestLocalizationBundleVersion = metadata.Value?.LatestLocalizationBundleVersion;

        var cachedLocalVersion = await _cache.GetAsync<string>(CachedLocalKey, null, context.CancellationToken);
        var cachedGameDataVersion = await _cache.GetAsync<string>(CachedGameDataKey, null, context.CancellationToken);

        if (latestGameDataVersion is null || latestLocalizationBundleVersion is null)
            return;

        if (cachedLocalVersion is not null && cachedGameDataVersion is not null &&
            latestGameDataVersion == cachedGameDataVersion &&
            latestLocalizationBundleVersion == cachedLocalVersion)
            return;

        if (metadata.Value is null)
            return;

        var result = await _sender.Send(
            new UpdateRawDataCommand(metadata.Value),
            context.CancellationToken);
        if (result.IsFailure)
            return;

        await _cache.SetAsync(
            CachedLocalKey,
            latestLocalizationBundleVersion,
            TimeSpan.MaxValue,
            null,
            context.CancellationToken);

        await _cache.SetAsync(
            CachedGameDataKey,
            latestGameDataVersion,
            TimeSpan.MaxValue,
            null,
            context.CancellationToken);
    }
}