namespace BuildingBlocks.Infrastructure.InternalCommand;

public interface IInternalCommandsMapper
{
    string GetName(Type type);

    Type GetType(string name);
}