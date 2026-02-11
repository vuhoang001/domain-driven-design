using MediatR;

namespace BuildingBlocks.Application.Configuration.Queries;

public interface IQuery<out TResult> : IRequest<TResult>
{
    
}