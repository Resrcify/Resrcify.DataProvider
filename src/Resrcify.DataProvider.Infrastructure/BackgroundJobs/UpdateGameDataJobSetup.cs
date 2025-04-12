using System;
using Microsoft.Extensions.Options;
using Quartz;

namespace Resrcify.DataProvider.Infrastructure.BackgroundJobs;

public class UpdateGameDataJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = new JobKey(nameof(UpdateGameDataJob));

        options
            .AddJob<UpdateGameDataJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(
                trigger =>
                    trigger.ForJob(jobKey)
                        .StartAt(DateTime.UtcNow.AddSeconds(30))
                        .WithSimpleSchedule(
                            schedule =>
                                schedule.WithIntervalInMinutes(15)
                                    .RepeatForever()));
    }
}