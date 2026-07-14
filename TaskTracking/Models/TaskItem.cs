namespace TaskTracking.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

}
