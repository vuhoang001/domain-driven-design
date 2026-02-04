using MediatR;
using Procurement.Application.Contracts;

namespace Procurement.Application.Configuration.Queries;

public interface IQueryHandle<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    
}