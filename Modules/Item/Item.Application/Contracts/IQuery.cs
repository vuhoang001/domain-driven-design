using MediatR;

namespace Item.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}