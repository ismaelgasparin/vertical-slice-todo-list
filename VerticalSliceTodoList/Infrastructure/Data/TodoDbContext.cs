using Microsoft.EntityFrameworkCore;
using VerticalSliceTodoList.Domain;

namespace VerticalSliceTodoList.Infrastructure.Data;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; }
}