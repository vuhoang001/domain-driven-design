using Autofac;
using BuildingBlocks.Application.Events;
using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Domain;
using BuildingBlocks.Infrastructure.Serialization;
using MediatR;
using Newtonsoft.Json;

namespace BuildingBlocks.Infrastructure.DomainEventsDispatching;

/// <summary>
/// Trung tam điều phối domain events. có nhiệm vụ:
/// + Publish domain events qua MediaR (xử lý ngay)
/// + Lưu notifications vào Outbox (xử lý sau, đảm bảo không mất events)
/// </summary>
/// <param name="mediator"></param>
/// <param name="scope"></param>
/// <param name="outbox"></param>
/// <param name="domainEventAccessor"></param>
/// <param name="domainNotificationMapper"></param>
public class DomainEventDispatcher(
    IMediator mediator,
    ILifetimeScope scope, // để resolve domain event notification.
    IOutbox outbox,
    IDomainEventAccessor domainEventAccessor,
    IDomainNotificationMapper domainNotificationMapper) : IDomainEventDispatcher
{
    public async Task DispatchEventAsync()
    {
        var domainEvents = domainEventAccessor.GetAllDomainEvents();

        List<IDomainEventNotification<IDomainEvent>> domainEventNotifications = [];
        foreach (var domainEvent in domainEvents)
        {
            /*
             * Đoạn này dùng refleaction kết hợp với DI (Autofac) để tạo đúng kiểu notification cho từng domain event khi chạy.
             *
             * B1: lấy template generic IDomainEventNotification<> rồi điền type cụ thể của event bằng MakeGenericType(domainEvent.GetType)
             *     Ví dụ: nết event là OrderCreatedEvent thì tạo ra IDomainEventNotification<OrderCreatedEvent>.
             * B2: Dùng scope.ResolveOptional để nhờ Autofac khởi tạo instance notirication tương ứng (ví dụ OrderCreatedNotifidcation).
             *     Các tham số constructor được truyền bằng tên qua NamedParameter: domainEvent và id.
             * B3; Nếu resolve thành công, add vào danh sách domainEventNotifications để serialize và lưu vào outbox.
             * => Publish tất cả domainEvents qua Mediar -> Xử lý ngay.
             * => Map tên type, serialize domainEventNotifications và lưu vào Outbox => xử lý sau bởi background job.
             */
            var domainEvenNotificationType        = typeof(IDomainEventNotification<>);
            var domainNotificationWithGenericType = domainEvenNotificationType.MakeGenericType(domainEvent.GetType());
            var domainNotification = scope.ResolveOptional(domainNotificationWithGenericType, new List<NamedParameter>
            {
                new NamedParameter("domainEvent", domainEvent),
                new NamedParameter("id", domainEvent.Id)
            });

            if (domainNotification != null)
            {
                domainEventNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent> ??
                                             throw new InvalidOperationException());
            }
        }

        domainEventAccessor.ClearAllDomainEvents();

        /*
         * Publish tất cả domainEvents qua Mediar => xử lý ngay.
         */
        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        /*
         * Map tên type, serialize domainEventNotifications và lưu vào Outbox => xử lý sau bởi background job.
         */
        foreach (var domainEventNotification in domainEventNotifications)
        {
            var type = domainNotificationMapper.GetName(domainEventNotification.GetType());
            var data = JsonConvert.SerializeObject(domainEventNotification, new JsonSerializerSettings()
            {
                ContractResolver = new AllPropertiesContractResolver()
            });

            var outboxMessage = new OutboxMessage(
                domainEventNotification.Id,
                domainEventNotification.DomainEvent.OccurredOn,
                type,
                data);

            outbox.Add(outboxMessage);
        }
    }
}