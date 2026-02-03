using BuildingBlocks.Application;
using FluentValidation;
using MasterData.Application.Configuration.Commands;
using MasterData.Application.Configuration.Contracts;
using MasterData.Application.Contracts;

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