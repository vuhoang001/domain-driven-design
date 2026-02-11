using BuildingBlocks.Application.Configuration.Commands;

namespace BuildingBlocks.Application.Contracts.Commands;

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