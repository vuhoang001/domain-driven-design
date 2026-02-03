using MasterData.Application.Contracts;

namespace MasterData.Application.Configuration.Contracts;

public class QueryBase<TResult> : IQuery<TResult>
{
    public Guid Id { get; }

    protected QueryBase()
    {
        Id = Guid.NewGuid();
    }

    protected QueryBase(Guid id)
    {
        Id = id;
    }
}