using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSliceTodoList.Infrastructure.Data;

namespace VerticalSliceTodoList.Features.TodoItems.Commands;

public record UpdateTodoItemCommand : IRequest<Result>
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand, Result>
{
    private readonly TodoDbContext _context;

    public UpdateTodoItemCommandHandler(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _context.TodoItems.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (todoItem is null)
            return Result.Fail("Todo item not found.");

        todoItem.Update(request.Title);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}

public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);
    }
}