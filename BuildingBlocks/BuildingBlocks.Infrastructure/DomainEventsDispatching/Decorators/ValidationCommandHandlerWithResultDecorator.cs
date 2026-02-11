using BuildingBlocks.Application;
using BuildingBlocks.Application.Configuration.Commands;
using FluentValidation;

namespace BuildingBlocks.Infrastructure.DomainEventsDispatching.Decorators;

public class ValidationCommandHandlerWithResultDecorator<T, TResult>(
    IList<IValidator<T>> validators,
    ICommandHandler<T, TResult> decorated) : ICommandHandler<T, TResult> where T : ICommand<TResult>
{
    public async Task<TResult> Handle(T request, CancellationToken cancellationToken)
    {
        var errors = validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (errors.Any())
        {
            throw new InvalidCommandException(errors.Select(x => x.ErrorMessage).ToList());
        }

        return await decorated.Handle(request, cancellationToken);
    }
}