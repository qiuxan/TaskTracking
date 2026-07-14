namespace TaskTracking.Dtos;

public class CreateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
}
