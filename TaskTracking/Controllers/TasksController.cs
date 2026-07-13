using Microsoft.AspNetCore.Mvc;
using TaskTracking.Models;

namespace TaskTracking.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private static readonly List<TaskItem> _taskItems =
            [
                new TaskItem
                {
                    Id = 1,
                    Title = "Review LINQ",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Build Tasks API",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                },
                new TaskItem
                {
                    Id = 3,
                    Title = "Check React",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                },
            ];
    
    [HttpGet]
    public ActionResult<List<TaskItem>> GetTasks()
    {
        return Ok(_taskItems);
    }

    [HttpGet("{id:int}")]
    public ActionResult<TaskItem> GetTaskById(int id)
    {
        var taskItem = _taskItems.FirstOrDefault(i => i.Id == id);

        if (taskItem is null) return NotFound();
        
        return Ok(taskItem);

    }

    [HttpPost]
    public ActionResult<TaskItem> CreateTask([FromBody] TaskItem taskItemToCreate)
    {
        if(string.IsNullOrEmpty(taskItemToCreate.Title))
            return BadRequest("Title is required");

        var existing = _taskItems.FirstOrDefault(t => t.Id == taskItemToCreate.Id);

        var taskItem = new TaskItem
        {
            Id = _taskItems.Max(i => i.Id) + 1,
            IsCompleted = false,
            Title = taskItemToCreate.Title,
            CreatedAt = DateTime.UtcNow
        };

        _taskItems.Add(taskItem);
        return CreatedAtAction(nameof(GetTaskById),new { id = taskItem.Id }, taskItem);

    }

}