using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BuildingBlocks.Infrastructure;

public class StronglyTypeIdValueConverterSelector(ValueConverterSelectorDependencies dependencies)
    : ValueConverterSelector(dependencies)
{
    
    private static Type? UnwrapNullableType(Type? type)
    {
        if (type is null)
        {
            return null;
        }

        return Nullable.GetUnderlyingType(type) ?? type;
    }
}