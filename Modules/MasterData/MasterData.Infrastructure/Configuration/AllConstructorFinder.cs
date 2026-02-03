using System.Collections.Concurrent;
using System.Reflection;
using Autofac.Core.Activators.Reflection;

namespace Item.Infrastructure.Configuration;

/// <summary>
/// Tìm tất cả các constructors của một type để Autofac có thể sử dụng khi tạo instance.
///
/// Chi tiết:
///     + IConstructorFinder: Interface của Autofac để xác định constructor nào sẽ sử dụng cho dependency injection.
///     + FindConstructor(): Phương thức tìm tất cả constructors có sẵn của một type.
/// </summary>
public class AllConstructorFinder : IConstructorFinder
{
    private static readonly ConcurrentDictionary<Type, ConstructorInfo[]> Cache = new();

    public ConstructorInfo[] FindConstructors(Type targetType)
    {
        var result = Cache.GetOrAdd(targetType, t => t.GetTypeInfo().DeclaredConstructors.ToArray());
        return result.Length > 0 ? result : throw new NoConstructorsFoundException(targetType, this);
    }
}