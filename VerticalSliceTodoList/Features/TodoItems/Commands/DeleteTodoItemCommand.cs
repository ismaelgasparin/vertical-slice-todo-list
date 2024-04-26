using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSliceTodoList.Infrastructure.Data;

namespace VerticalSliceTodoList.Features.TodoItems.Commands;

public record DeleteTodoItemCommand : IRequest<Result>
{
    public Guid Id { get; init; }
}

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand, Result>
{
    private readonly TodoDbContext _context;

    public DeleteTodoItemCommandHandler(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _context.TodoItems.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (todoItem is null)
            return Result.Fail("Todo item not found.");

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
