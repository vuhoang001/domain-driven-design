using BuildingBlocks.Application;

namespace ddd.API.Configuration.ExecutionContext;

public class ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor) : IExecutionContextAccessor
{
    /// <summary>
    /// Lấy UserId của user hiện tại từ claims trong HttpContext.
    /// </summary>
    /// <exception cref="ApplicationException"></exception>
    public Guid UserId
    {
        get
        {
            if (httpContextAccessor.HttpContext?
                .User?
                .Claims?
                .SingleOrDefault(x => x.Type == "sub")?
                .Value != null)
            {
                return Guid.Parse(httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == "sub").Value);
            }

            throw new ApplicationException("User is not available");
        }
    }

    /// <summary>
    /// Lấy thông tin CorrelationId từ header request được thêm bởi CorrelationMiddleware.
    /// Nếu không có => throw exception
    /// </summary>
    /// <exception cref="ApplicationException"></exception>
    public Guid CorrelationId
    {
        get
        {
            if (IsAvailable &&
                (bool)httpContextAccessor.HttpContext?.Request.Headers.Keys.Any(x => x == CorrelationMiddleware
                        .CorrelationHeaderKey))
            {
                return Guid.Parse(
                    httpContextAccessor.HttpContext.Request.Headers[CorrelationMiddleware.CorrelationHeaderKey]);
            }

            throw new ApplicationException("Http context and correlation id is not available");
        }
    }

    /// <summary>
    /// Kiểm tra xem có HTTP context hiện tại không (VD: gọi từ request HTTP hay từ background job)
    /// </summary>
    public bool IsAvailable => httpContextAccessor.HttpContext != null;
}