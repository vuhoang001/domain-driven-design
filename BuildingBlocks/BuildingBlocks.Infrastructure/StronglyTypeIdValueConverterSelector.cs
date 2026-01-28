using System.Collections.Concurrent;
using BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BuildingBlocks.Infrastructure;

/// <summary>
/// Dựa vào https://andrewlock.net/strongly-typed-ids-in-ef-core-using-strongly-typed-entity-ids-to-avoid-primitive-obsession-part-4.
/// </summary>
/// <param name="dependencies"></param>
public class StronglyTypeIdValueConverterSelector(ValueConverterSelectorDependencies dependencies)
    : ValueConverterSelector(dependencies)
{
    private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> _converters
        = new();

    public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type? providerClrType = null)
    {
        var baseConvertes = base.Select(modelClrType, providerClrType);
        foreach (var valueConverterInfo in baseConvertes)
        {
            yield return valueConverterInfo;
        }

        var underlyingModelType    = UnwrapNullableType(modelClrType);
        var underlyingProviderType = UnwrapNullableType(providerClrType);

        if (underlyingProviderType is null || underlyingProviderType == typeof(Guid))
        {
            var isTypedIdValue = typeof(TypeIdValueBase).IsAssignableFrom(underlyingModelType!);
            if (isTypedIdValue)
            {
                var converterType = typeof(TypeIdValueConverter<>).MakeGenericType(underlyingModelType!);

                yield return _converters.GetOrAdd((underlyingModelType!, typeof(Guid)), _ =>
                {
                    return new ValueConverterInfo(
                        modelClrType: modelClrType,
                        providerClrType: typeof(Guid),
                        factory: valueConverterInfo =>
                            (ValueConverter)Activator.CreateInstance(
                                converterType, valueConverterInfo.MappingHints)!);
                });
            }
        }
    }

    private static Type? UnwrapNullableType(Type? type)
    {
        if (type is null)
        {
            return null;
        }

        return Nullable.GetUnderlyingType(type) ?? type;
    }
}