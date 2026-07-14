using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracking.Data;
using TaskTracking.Dtos;
using TaskTracking.Models;

namespace TaskTracking.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<TaskResponse>>> GetTasks()
    {
        var taskList = await _context.Tasks.ToListAsync();
       
        var response = taskList.Select(MapTaskItemToTaskResponse).ToList();
            
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponse>> GetTaskById(int id)
    {
        var taskItem = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        if (taskItem is null) return NotFound();

        var response = MapTaskItemToTaskResponse(taskItem);
        
        return Ok(response);

    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> CreateTask([FromBody] CreateTaskRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("Title is required");
        
        var taskItem = new TaskItem
        {
            IsCompleted = false,
            Title = request.Title.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(taskItem);
        await _context.SaveChangesAsync();
        
        TaskResponse response = MapTaskItemToTaskResponse(taskItem);
        
        return CreatedAtAction(nameof(GetTaskById),new { id = response.Id }, response);

    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest taskItemToUpdateRequest)
    {
        var taskExisting = await _context.Tasks.FirstOrDefaultAsync(t=>t.Id == id);
        
        if(taskExisting is null)
            return NotFound();
        
        if (string.IsNullOrWhiteSpace(taskItemToUpdateRequest.Title))
            return BadRequest("Title is required");
        
        taskExisting.Title = taskItemToUpdateRequest.Title.Trim();
        taskExisting.IsCompleted = taskItemToUpdateRequest.IsCompleted;
        
        await _context.SaveChangesAsync();
        return NoContent();
        
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var existingTaskItem = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        if (existingTaskItem is null)
            return NotFound();
        
        _context.Tasks.Remove(existingTaskItem);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static TaskResponse MapTaskItemToTaskResponse(TaskItem item)
    {
        return new TaskResponse
        {
            Id = item.Id,
            Title = item.Title,
            CreatedAt = item.CreatedAt,
            IsCompleted = item.IsCompleted,
        };
    }
}