namespace Item.Application.Contracts;

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