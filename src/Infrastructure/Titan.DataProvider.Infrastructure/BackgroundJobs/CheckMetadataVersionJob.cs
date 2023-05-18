using System.Threading.Tasks;
using MediatR;
using Quartz;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Application.Features.Data.Commands.UpdateRawDataFromTitan;
using Titan.DataProvider.Application.Features.Data.Queries.GetMetadataVersion;

namespace Titan.TournamentManagement.Infrastructure.BackgroundJobs;

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
        var latestGamedataVersion = metadata.Value?.LatestGamedataVersion?.Split(":")[1];
        var latestLocalizationBundleVersion = metadata.Value?.LatestLocalizationBundleVersion;

        var cachedLocalVersion = await _cache.GetAsync<string>("LatestLocalizationBundleVersion", context.CancellationToken);
        var cachedGamedataVersion = await _cache.GetAsync<string>("LatestGamedataVersion", context.CancellationToken);

        if (metadata.IsFailure || latestGamedataVersion is null || latestLocalizationBundleVersion is null)
            return;

        if (cachedLocalVersion is not null && cachedGamedataVersion is not null &&
            latestGamedataVersion == cachedGamedataVersion &&
            latestLocalizationBundleVersion == cachedLocalVersion)
            return;

        var result = await _sender.Send(new UpdateRawDataFromTitanCommand());
        if (result.IsFailure) return;
        await _cache.SetAsync("LatestLocalizationBundleVersion", latestLocalizationBundleVersion, context.CancellationToken);
        await _cache.SetAsync("LatestGamedataVersion", latestGamedataVersion, context.CancellationToken);


    }
}