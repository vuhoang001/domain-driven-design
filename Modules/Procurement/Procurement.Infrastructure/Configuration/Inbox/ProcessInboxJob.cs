using Quartz;

namespace Procurement.Infrastructure.Configuration.Inbox;

[DisallowConcurrentExecution]
public class ProcessInboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandsExecutor.Execute(new ProcessInboxCommand());
    }
}