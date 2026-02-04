namespace Procurement.Application.Contracts;

public class InternalCommandBase : ICommand
{
    protected InternalCommandBase()
    {
        Id = Guid.NewGuid();
    }

    protected InternalCommandBase(Guid id)
    {
        Id = id;
    }
    public Guid Id { get;  }
}

public abstract class InternalCommandBase<T> : ICommand<T>
{
    protected InternalCommandBase()
    {
        Id = Guid.NewGuid();
    }

    protected InternalCommandBase(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}