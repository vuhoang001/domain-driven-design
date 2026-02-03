using Item.Infrastructure.Configuration.Processing.InternalCommands;
using Item.Infrastructure.Configuration.Processing.Outbox;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Serilog;

namespace Item.Infrastructure.Configuration.Quartz;

public static class QuartzStartUp
{
    private static IScheduler _scheduler;

    internal static void Initialize(ILogger logger, long? internalProcessingPoolingInterval)
    {
        logger.Information("Quartz starting...");

        var schedulerConfiguration = new System.Collections.Specialized.NameValueCollection();
        schedulerConfiguration.Add("quartz.scheduler.instanceName", "Item");
        ISchedulerFactory schedulerFactory = new StdSchedulerFactory(schedulerConfiguration);
        _scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
        LogProvider.SetCurrentLogProvider(new SerilogLogProvider(logger));
        _scheduler.Start().GetAwaiter().GetResult();


        #region OutboxProcessing

        var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();

        ITrigger trigger;
        if (internalProcessingPoolingInterval.HasValue)
        {
            trigger =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithSimpleSchedule(x =>
                                            x.WithInterval(
                                                    TimeSpan.FromMilliseconds(internalProcessingPoolingInterval.Value))
                                                .RepeatForever())
                    .Build();
        }
        else
        {
            trigger = TriggerBuilder
                .Create()
                .StartNow()
                .WithCronSchedule("0/2 * * ? * *")
                .Build();
        }

        _scheduler
            .ScheduleJob(processOutboxJob, trigger)
            .GetAwaiter().GetResult();

        #endregion

        #region InternalCommandsProcessing

        var processInternalCommandsJob = JobBuilder.Create<ProcessInternalCommandJob>().Build();

        ITrigger triggerCommandsProcessing;

        if (internalProcessingPoolingInterval.HasValue)
        {
            triggerCommandsProcessing =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithSimpleSchedule(x =>
                                            x.WithInterval(
                                                    TimeSpan.FromMilliseconds(internalProcessingPoolingInterval.Value))
                                                .RepeatForever())
                    .Build();
        }
        else
        {
            triggerCommandsProcessing =
                TriggerBuilder
                    .Create()
                    .StartNow()
                    .WithCronSchedule("0/2 * * ? * *")
                    .Build();
        }

        _scheduler.ScheduleJob(processInternalCommandsJob, triggerCommandsProcessing).GetAwaiter().GetResult();

        #endregion
    }

    internal static void StopQuartz()
    {
        _scheduler?.Shutdown();
    }
}