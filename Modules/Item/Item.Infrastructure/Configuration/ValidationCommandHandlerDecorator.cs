using BuildingBlocks.Application;
using Item.Application.Configuration.Commands;
using Item.Application.Configuration.Contracts;
using FluentValidation;
using Item.Application.Contracts;

namespace Item.Infrastructure.Configuration;

public class ValidationCommandHandlerDecorator<T>(IList<IValidator<T>> validators, ICommandHandler<T> decorated)
    : ICommandHandler<T> where T : ICommand
{
    public async Task Handle(T request, CancellationToken cancellationToken)
    {
        var errors = validators.Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (errors.Count > 0)
        {
            throw new InvalidCommandException(errors.Select(x => x.ErrorMessage).ToList());
        }

        await decorated.Handle(request, cancellationToken);
    }
}