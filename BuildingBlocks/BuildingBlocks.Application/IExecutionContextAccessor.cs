namespace BuildingBlocks.Application;

/// <summary>
/// Cung cấp thông tin context của request hiện tại đang được thực thi.
/// là cầu nối để lấy thông tin về request hiện tại (ai gửi request, request này có ID gì, ...)
/// </summary>
public interface IExecutionContextAccessor
{
    /// <summary>
    /// Guid định danh duy nhất cho mỗi user.
    /// </summary>
    Guid UserId { get; }

    /// <summary>
    /// Guid định danh duy nhất cho mỗi request.
    /// </summary>
    Guid CorrelationId { get; }

    /// <summary>
    /// Xác định xem context hiện tại có sẵn hay không.
    /// </summary>
    bool IsAvailable { get; }
}