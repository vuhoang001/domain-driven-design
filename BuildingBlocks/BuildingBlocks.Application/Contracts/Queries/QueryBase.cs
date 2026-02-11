using BuildingBlocks.Application.Configuration.Queries;

namespace BuildingBlocks.Application.Contracts.Queries;

public class QueryBase<TResult> : IQuery<TResult>
{
    public Guid Id { get; set; }

    protected QueryBase()
    {
        Id = Guid.NewGuid();
    }

    protected QueryBase(Guid id)
    {
        Id = id;
    }
}