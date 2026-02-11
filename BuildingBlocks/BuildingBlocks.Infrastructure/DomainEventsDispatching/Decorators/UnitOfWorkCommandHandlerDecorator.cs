using BuildingBlocks.Application.Configuration.Commands;

namespace BuildingBlocks.Infrastructure.DomainEventsDispatching.Decorators;

public class UnitOfWorkCommandHandlerDecorator<T>(ICommandHandler<T> decorated, IUnitOfWork unitOfWork)
    : ICommandHandler<T> where T : ICommand
{
    public async Task Handle(T request, CancellationToken cancellationToken)
    {
       await decorated.Handle(request, cancellationToken); 
       
       await unitOfWork.CommitAsync(cancellationToken);
    }
}