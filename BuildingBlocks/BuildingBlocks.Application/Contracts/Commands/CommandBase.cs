using BuildingBlocks.Application.Configuration.Commands;

namespace BuildingBlocks.Application.Contracts.Commands;

public abstract class CommandBase(Guid id) : ICommand
{
    public Guid Id { get; } = id;

    protected CommandBase() : this(Guid.NewGuid())
    {
    }
}

public abstract class CommandBase<TResult>(Guid id) : ICommand<TResult>
{
    protected CommandBase() : this(Guid.NewGuid())
    {
    }

    public Guid Id { get; } = id;
}