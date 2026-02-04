using MediatR;

namespace Procurement.Application.Contracts;

public interface ICommand : IRequest
{
}

public interface ICommand<out TResult> : IRequest<TResult>
{
    public Guid Id { get; }
}