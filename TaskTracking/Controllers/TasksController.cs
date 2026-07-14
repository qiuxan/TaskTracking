using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracking.Data;
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
    public async Task<ActionResult<List<TaskItem>>> GetTasks()
    {
        var taskList = await _context.Tasks.ToListAsync();
        return Ok(taskList);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskItem>> GetTaskById(int id)
    {
        var taskItem = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        if (taskItem is null) return NotFound();
        
        return Ok(taskItem);

    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTask([FromBody] TaskItem taskItemToCreate)
    {
        if(string.IsNullOrEmpty(taskItemToCreate.Title))
            return BadRequest("Title is required");
        
        var taskItem = new TaskItem
        {
            IsCompleted = false,
            Title = taskItemToCreate.Title,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(taskItem);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetTaskById),new { id = taskItem.Id }, taskItem);

    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem taskItemToUpdate)
    {
        var taskExisting = await _context.Tasks.FirstOrDefaultAsync(t=>t.Id == id);
        
        if(taskExisting is null)
            return NotFound();
        
        if (string.IsNullOrWhiteSpace(taskItemToUpdate.Title))
            return BadRequest("Title is required");
        
        taskExisting.Title = taskItemToUpdate.Title.Trim();
        taskExisting.IsCompleted = taskItemToUpdate.IsCompleted;
        
        await _context.SaveChangesAsync();
        return NoContent();
        
    }
    
}