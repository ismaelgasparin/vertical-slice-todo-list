using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSliceTodoList.Domain;
using VerticalSliceTodoList.Infrastructure.Data;

namespace VerticalSliceTodoList.Features.TodoItems.Queries;

public record GetTodoListQuery : IRequest<Result<List<TodoItem>>>
{
    public bool OnlyNotCompleted { get; init; } = false;
}

public class GetTodoListQueryHandler : IRequestHandler<GetTodoListQuery, Result<List<TodoItem>>>
{
    private readonly TodoDbContext _context;

    public GetTodoListQueryHandler(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<TodoItem>>> Handle(GetTodoListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.TodoItems.AsQueryable();

        if (request.OnlyNotCompleted)
        {
            query = query.Where(i => !i.IsCompleted);
        }

        query = query.OrderBy(i => i.CreatedAt);

        var items = await query.ToListAsync(cancellationToken);

        return Result.Ok(items);
    }
}

