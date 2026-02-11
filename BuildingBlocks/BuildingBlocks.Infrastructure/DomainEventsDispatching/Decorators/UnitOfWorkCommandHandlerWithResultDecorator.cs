using BuildingBlocks.Application.Configuration.Commands;

namespace BuildingBlocks.Infrastructure.DomainEventsDispatching.Decorators;

public class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult>(
    ICommandHandler<T, TResult> decorated,
    IUnitOfWork unitOfWork) : ICommandHandler<T, TResult> where T : ICommand<TResult>
{
    public async Task<TResult> Handle(T request, CancellationToken cancellationToken)
    {
        var result = await decorated.Handle(request, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);
        return result;
    }
}