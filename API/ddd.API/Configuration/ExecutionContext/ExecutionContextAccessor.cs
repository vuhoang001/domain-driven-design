using BuildingBlocks.Application;

namespace ddd.API.Configuration.ExecutionContext;

public class ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor) : IExecutionContextAccessor
{
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

    public bool IsAvailable => httpContextAccessor.HttpContext != null;
}