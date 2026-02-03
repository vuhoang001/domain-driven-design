namespace MasterData.Application.Contracts;

/// <summary>
/// Sử dụng CommandBase khi: 
/// + Cần kết quả ngay lập tức (synchronous).
/// + Nếu lỗi => rollback toàn bộ
/// + Là entry point từ đầu đến cuối
/// </summary>
/// <typeparam name="TResult"></typeparam>
public class CommandBase<TResult> : ICommand<TResult>
{
    protected CommandBase(Guid id)
    {
        Id = id;
    }

    protected CommandBase()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
}

public class CommandBase : ICommand
{
    protected CommandBase(Guid id)
    {
        Id = id;
    }

    protected CommandBase()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
}