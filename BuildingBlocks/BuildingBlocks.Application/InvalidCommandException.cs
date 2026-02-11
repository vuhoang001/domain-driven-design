namespace BuildingBlocks.Application;

public class InvalidCommandException(List<string> errors) : Exception
{
    public List<string> Errors { get; } = errors;
}