using FluentResults;
using FluentValidation;
using MediatR;
using VerticalSliceTodoList.Domain;
using VerticalSliceTodoList.Infrastructure.Data;

namespace VerticalSliceTodoList.Features.TodoItems.Commands;

public record AddTodoItemCommand : IRequest<Result<Guid>>
{
    public string Title { get; init; } = string.Empty;
}

public class AddTodoItemCommandHandler : IRequestHandler<AddTodoItemCommand, Result<Guid>>
{
    private readonly TodoDbContext _context;

    public AddTodoItemCommandHandler(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = new TodoItem(request.Title);

        await _context.TodoItems.AddAsync(todoItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok(todoItem.Id);
    }
}

public class AddTodoItemCommandValidator : AbstractValidator<AddTodoItemCommand>
{
    public AddTodoItemCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);
    }
}
