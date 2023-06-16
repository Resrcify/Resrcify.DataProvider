using System.Threading.Tasks;
using MediatR;
using Quartz;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Application.Features.Data.Commands.UpdateRawData;
using Titan.DataProvider.Application.Features.Data.Queries.GetMetadataVersion;

namespace Titan.DataProvider.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class CheckMetadataVersionJob : IJob
{
    private readonly ICachingService _cache;
    private readonly ISender _sender;

    public CheckMetadataVersionJob(ICachingService cache, ISender sender)
    {
        _cache = cache;
        _sender = sender;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var metadata = await _sender.Send(new GetMetadataVersionQuery(), context.CancellationToken);
        if (metadata.IsFailure) return;
        var latestGameDataVersion = metadata.Value?.LatestGamedataVersion?.Split(":")[1];
        var latestLocalizationBundleVersion = metadata.Value?.LatestLocalizationBundleVersion;

        var cachedLocalVersion = await _cache.GetAsync<string>("LatestLocalizationBundleVersion", context.CancellationToken);
        var cachedGameDataVersion = await _cache.GetAsync<string>("LatestGameDataVersion", context.CancellationToken);

        if (latestGameDataVersion is null || latestLocalizationBundleVersion is null)
            return;

        if (cachedLocalVersion is not null && cachedGameDataVersion is not null &&
            latestGameDataVersion == cachedGameDataVersion &&
            latestLocalizationBundleVersion == cachedLocalVersion)
            return;

        var result = await _sender.Send(new UpdateRawDataCommand());
        if (result.IsFailure) return;
        await _cache.SetAsync("LatestLocalizationBundleVersion", latestLocalizationBundleVersion, context.CancellationToken);
        await _cache.SetAsync("LatestGameDataVersion", latestGameDataVersion, context.CancellationToken);


    }
}