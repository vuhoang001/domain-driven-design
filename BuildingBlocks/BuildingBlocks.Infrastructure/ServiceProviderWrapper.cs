using Autofac;

namespace BuildingBlocks.Infrastructure;

/// <summary>
/// Bài toán:
/// - Trong nhiều chỗ của framework .net (đặc biệt là ASP.net Core, background services, ...), người ta kỳ vọng nhận được implementation của IServiceProvider.
/// - Nhưng bạn lại dùng Autofac (với ILifetimeScope) thay vì build-in DI container của Microsoft.
/// Giải pháp:
/// - ServiceProvider đóng vai trò bộ chuyển đổi (adapter) giữa:
/// </summary>
/// <param name="scope"></param>
public class ServiceProviderWrapper(ILifetimeScope scope) : IServiceProvider
{
    public object? GetService(Type serviceType) => scope.ResolveOptional(serviceType);
}