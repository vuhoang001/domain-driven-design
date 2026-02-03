using BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BuildingBlocks.Infrastructure;

public class TypeIdValueConverter<TTypedIdValue>(ConverterMappingHints? mappingHints = null)
    : ValueConverter<TTypedIdValue, Guid>(id => id.Value, value => Create(value), mappingHints)
    where TTypedIdValue : TypeIdValueBase
{
    private static TTypedIdValue Create(Guid id) =>
        (TTypedIdValue)Activator.CreateInstance(typeof(TTypedIdValue), id)!;
}