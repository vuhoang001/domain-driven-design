namespace ddd.API.Configuration.ExecutionContext;

/// <summary>
/// Là một middleware để theo dõi request từ đầu đến cuối.
/// Mỗi lần có request đến, middleware này sẽ tạo một CorrelationId mới và thêm nó vào header của request.
///
/// Theo dõi Client gửi request đến API.
/// 1. Client gửi request đến API
/// 2. CorrelationMiddleware tạo CorrelationId = "abc123"
/// 3. Thêm header : CorrelationId: abc123
/// 4. Controller xử lý
/// 5. Ghi log kèm CorrelationId
/// 6. Database operation kèm CorrelationId: abc123
/// 7. Response trả về client kèm CorrelationId: abc123
/// => Có thể tìm tất cả log/event của 1 request dựa vào CorrelationId duy nhất.
/// </summary>
/// <param name="next"></param>
public class CorrelationMiddleware(RequestDelegate next)
{
    internal const string CorrelationHeaderKey = "CorrelationId";

    public async Task Invoke(HttpContext context)
    {
        var correlationId = Guid.NewGuid();

        context.Request?.Headers.Append(CorrelationHeaderKey, correlationId.ToString());

        await next.Invoke(context);
    }
}