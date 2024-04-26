namespace VerticalSliceTodoList.Domain;

public class TodoItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public TodoItem(string title)
    {
        Id = Guid.NewGuid();
        Title = title;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string title)
    {
        Title = title;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAs(bool isCompleted)
    {
        IsCompleted = isCompleted;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        DeletedAt = DateTime.UtcNow;
    }
}
