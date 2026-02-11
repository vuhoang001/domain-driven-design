using BuildingBlocks.Application;
using BuildingBlocks.Application.Configuration.Commands;
using FluentValidation;

namespace BuildingBlocks.Infrastructure.DomainEventsDispatching.Decorators;

public class ValidationCommandHandlerDecorator<T>(IList<IValidator<T>> validators, ICommandHandler<T> decorated)
    : ICommandHandler<T> where T : ICommand
{
    public async Task Handle(T request, CancellationToken cancellationToken)
    {
        var errors = validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (errors.Count != 0)
        {
            throw new InvalidCommandException(errors.Select(x => x.ErrorMessage).ToList());
        }

        await decorated.Handle(request, cancellationToken);
    }
}