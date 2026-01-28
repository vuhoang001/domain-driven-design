using MediatR;

namespace Item.Application.Configuration.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}