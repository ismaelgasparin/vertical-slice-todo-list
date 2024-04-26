using FluentResults;
using FluentValidation;
using MediatR;
using System;

namespace VerticalSliceTodoList.Infrastructure.Validations;

public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase<TResponse>, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if(!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context)));

        var errors = validationFailures
            .Where(item => !item.IsValid)
            .SelectMany(item => item.Errors)
            .Select(item => new Error(item.ErrorMessage)
                .WithMetadata("property", item.PropertyName))
            .ToList();

        if (errors.Count > 0)
        {
            return new TResponse()
                .WithErrors(errors);
        }

        return await next();
    }
}