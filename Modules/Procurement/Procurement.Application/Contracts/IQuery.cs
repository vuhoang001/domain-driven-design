using MediatR;

namespace Procurement.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}