namespace BuildingBlocks.Domain.SeedWork;

/// <summary>
/// Quản lý vòng đời của Aggregate Root.
/// </summary>
public interface IAggregateStore
{
    /// <summary>
    /// Lưu tất cả các aggregates changes vào trong database.
    /// </summary>
    /// <returns></returns>
    Task Save();

    /// <summary>
    /// Load Aggregate từ database bằng cách replay tất cả các domain events.
    /// Cách hoạt động:
    ///     1. Lấy streamId từ AggregateId.
    ///     2. Query event stream table (sqlStreamSql).
    ///     3. Deresialize Json events thành domain events.
    ///     4. Create empty aggregate instance.
    ///     5. Replay tất cả các domain events (Apply events).
    ///     6. Return aggregate với state fully reconstructed.
    /// </summary>
    /// <param name="aggregateId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> Load<T>(AggregateId<T> aggregateId) where T : AggregateRoot;

    /// <summary>
    /// Lấy ra tất cả các domain events được thêm vào aggregates (Chưa lưu).
    /// </summary>
    /// <returns></returns>
    List<IDomainEvent> GetChanges();

    /// <summary>
    /// Track aggregate changes(Chuẩn bị để lưu).
    /// Cách hoạt động:
    ///     1. lấy domain events từ  (aggregate.GetDomainEvents).
    ///     2. Convert events thành stream messages (Serialize Json).
    ///     3. Thêm vào _aggregatesToSave()
    ///     4. Track events vào AppendChanges List.
    /// </summary>
    /// <param name="aggregate"></param>
    /// <typeparam name="T"></typeparam>
    void AppendChanges<T>(T aggregate) where T : AggregateRoot;
    /// <summary>
    /// Xóa các tracked changes
    /// </summary>
    void ClearChanges();
}