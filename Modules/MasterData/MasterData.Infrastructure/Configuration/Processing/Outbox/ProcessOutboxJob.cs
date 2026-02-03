using Quartz;

namespace Item.Infrastructure.Configuration.Processing.Outbox;

/// <summary>
/// [DisallowConcurrentExcecution] - Ngăn chặn thực thi đồng thời của job.
/// Tác dụng:
///     Attribute này đảm bảo rằng chỉ 1 instance của job này được chạy  1 thời điểm, tại ngay cả khi Quartz scheduler cố gắng chạy nhiều instance song song.
/// </summary>
[DisallowConcurrentExecution]
public class ProcessOutboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandsExecutor.Execute(new ProcessOutboxCommand());
    }
}