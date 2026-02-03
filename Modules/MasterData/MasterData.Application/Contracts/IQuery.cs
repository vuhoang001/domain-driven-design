using MediatR;

namespace MasterData.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
}