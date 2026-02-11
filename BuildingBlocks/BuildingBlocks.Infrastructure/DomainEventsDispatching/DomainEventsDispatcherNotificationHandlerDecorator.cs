using MediatR;

namespace BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    public class DomainEventsDispatcherNotificationHandlerDecorator<T>(
        IDomainEventsDispatcher domainEventsDispatcher,
        INotificationHandler<T> decorated)
        : INotificationHandler<T>
        where T : INotification
    {
        public async Task Handle(T notification, CancellationToken cancellationToken)
        {
            await decorated.Handle(notification, cancellationToken);

            await domainEventsDispatcher.DispatchEventsAsync();
        }
    }
}