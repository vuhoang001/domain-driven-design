using MediatR;

namespace BuildingBlocks.Infrastructure.DomainEventsDispatching;

/// <summary>
/// Trigger việc dispatch domain events sau khi xử lý notification xong.
///
/// Giải thích:
/// 1. Application handler (command handler / notification handler) thao tác aggregarte.
/// 2. Aggregate AddDomainEvent(...) các IDomainEvent (ví dụ: OrderCreatedDomainEvent).
/// 3. Sau khi handler chạy xong, ta muốn:
///     + Publish tất cả domain events qua Mediar (xử lý ngay).
///     + Đồng thời bọc chúng vào IDomainEventNotification và lưu vào Outbox (xử lý sau bởi background job, để đảm bảo không mất events).
/// => Docorator này chính là cặp đôi đẩm nhận bước 3 này, theo cách tự động và tập trung.
/// </summary>
/// <param name="decorator"></param>
/// <param name="domainEventDispatcher"></param>
/// <typeparam name="T"></typeparam>
public class DomainEventDispatcherNotificationHandlerDecorator<T>(
    INotificationHandler<T> decorator,
    IDomainEventDispatcher domainEventDispatcher) : INotificationHandler<T> where T : INotification
{
    public async Task Handle(T notification, CancellationToken cancellationToken)
    {
        // 1. Gọi handler thật để xử lý notification(ví dụ : handler của một domain evnet, integration event, v.v.)
        await decorator.Handle(notification, cancellationToken);
        // 2. Sau khi handler chạy xong, gọi dispatcher để: 
        //  + Lấy tất cả domain events đã được các aggregate/entity raise trong quá trình xử lý.
        //  + Tạo các notification tương ứng
        //  + Publish domain events qua MediatR (xử lý ngay)
        //  + Serial và lưu notification vào Outbox (xử lý sau)
        await domainEventDispatcher.DispatchEventAsync();
    }
}