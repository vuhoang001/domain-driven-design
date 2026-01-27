using BuildingBlocks.Domain;

namespace BuildingBlocks.Infrastructure.DomainEventsDispatching;

/// <summary>
/// Quản lý và truy cập các sự kiện miền (domain events) trong hệ thống.
/// </summary>
public interface IDomainEventAccessor
{
    /// <summary>
    /// Lấy tất cả các sự kiện miền hiện có.
    /// </summary>
    /// <returns></returns>
    IReadOnlyList<IDomainEvent> GetAllDomainEvents();

    /// <summary>
    /// Xóa tất cả các sự kiện miền đã được xử lý.
    /// </summary>
    void ClearAllDomainEvents();
}