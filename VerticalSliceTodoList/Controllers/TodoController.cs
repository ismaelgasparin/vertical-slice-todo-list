using MediatR;
using Microsoft.AspNetCore.Mvc;
using VerticalSliceTodoList.Features.TodoItems.Commands;
using VerticalSliceTodoList.Features.TodoItems.Queries;
using VerticalSliceTodoList.Infrastructure.Extensions;

namespace VerticalSliceTodoList.Controllers;

[ApiController]
[Route("todo")]
[Produces("application/json")]
public class TodoController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetTodoList(
        [FromQuery] bool onlyNotCompleted = false,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTodoListQuery { OnlyNotCompleted = onlyNotCompleted };

        var result = await _mediator.Send(query, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodoItem(Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTodoItemQuery { Id = id };

        var result = await _mediator.Send(query, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> AddTodoItem(
        [FromBody] AddTodoItemCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.ToActionResult(
            onSuccess: r =>
            {
                return new CreatedAtActionResult(
                    nameof(GetTodoItem), "todo", new { id = r.Value }, 
                    r.Value);
            });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodoItem(Guid id,
        [FromBody] UpdateTodoItemCommand command)
    {
        command = command with { Id = id };

        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("{id}/mark-as")]
    public async Task<IActionResult> UpdateTodoItem(Guid id,
        [FromBody] MarkAsTodoItemCommand command)
    {
        command = command with { Id = id };

        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(Guid id)
    {
        var command = new DeleteTodoItemCommand { Id = id };

        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
}