using BuildingBlocks.Application;
using FluentValidation;
using Item.Application.Configuration.Commands;
using Item.Application.Configuration.Contracts;
using Item.Application.Contracts;

namespace Item.Infrastructure.Configuration;

public class ValidationCommandHandlerWithResultDecorator<T, TResult>(
    IList<IValidator<T>> validators,
    ICommandHandler<T, TResult> decorated) : ICommandHandler<T, TResult> where T : ICommand<TResult>
{
    public async Task<TResult> Handle(T request, CancellationToken cancellationToken)
    {
        var errors = validators.Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (errors.Count > 0)
        {
            throw new InvalidCommandException(errors.Select(x => x.ErrorMessage).ToList());
        }

        var result = await decorated.Handle(request, cancellationToken);
        return result;
    }
}