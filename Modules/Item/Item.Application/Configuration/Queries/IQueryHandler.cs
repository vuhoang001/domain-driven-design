using Item.Application.Contracts;
using MediatR;

namespace Item.Application.Configuration.Queries;

public interface IQueryHandler<in TQuery,  TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    
}