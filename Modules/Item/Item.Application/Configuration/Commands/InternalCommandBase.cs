using Item.Application.Configuration.Contracts;
using Item.Application.Contracts;

namespace Item.Application.Configuration.Commands;
/// <summary>
/// Base class cho internal command -> dùng cho background jobs, outbox pattern , v.v.
/// </summary>
public abstract class InternalCommandBase(Guid id) : ICommand
{
    public Guid Id { get; } = id;
}

public abstract class InternalCommandBase<TResult>(Guid id) : ICommand<TResult>
{
    protected InternalCommandBase() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; } = id;
}