using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSliceTodoList.Domain;
using VerticalSliceTodoList.Infrastructure.Data;

namespace VerticalSliceTodoList.Features.TodoItems.Queries;

public record GetTodoItemQuery : IRequest<Result<TodoItem>>
{
    public Guid Id { get; init; }
}

public class GetTodoItemQueryHandler : IRequestHandler<GetTodoItemQuery, Result<TodoItem>>
{
    private readonly TodoDbContext _context;

    public GetTodoItemQueryHandler(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<Result<TodoItem>> Handle(GetTodoItemQuery request, CancellationToken cancellationToken)
    {
        var todoItem = await _context.TodoItems.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (todoItem is null)
            return Result.Fail("Todo item not found.");

        return Result.Ok(todoItem);
    }
}