using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSliceTodoList.Infrastructure.Data;

namespace VerticalSliceTodoList.Features.TodoItems.Commands;

public record MarkAsTodoItemCommand : IRequest<Result>
{
    public Guid Id { get; init; }
    public bool IsCompleted { get; init; }
}

public class MarkAsTodoItemCommandHandler : IRequestHandler<MarkAsTodoItemCommand, Result>
{
    private readonly TodoDbContext _context;

    public MarkAsTodoItemCommandHandler(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(MarkAsTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _context.TodoItems.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (todoItem is null)
            return Result.Fail("Todo item not found.");

        todoItem.MarkAs(request.IsCompleted);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}