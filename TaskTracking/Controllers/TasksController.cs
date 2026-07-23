using Microsoft.AspNetCore.Mvc;
using TaskTracking.Dtos;
using TaskTracking.Services;

namespace TaskTracking.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    public TasksController(
        ITaskService  taskService
        )
    {
        _taskService = taskService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<TaskResponse>>> GetTasks()
    {
        var response = await _taskService.GetTasksAsync();
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponse>> GetTaskById(int id)
    {
        var response =  await _taskService.GetTaskByIdAsync(id);
        if(response is null)
            return NotFound();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> CreateTask([FromBody] CreateTaskRequest request)
    {
        try
        {
            var response = await _taskService.CreateTaskAsync(request);

            return CreatedAtAction(nameof(GetTaskById), new { id = response.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest taskItemToUpdateRequest)
    {
        try
        {
            await _taskService.UpdateTaskAsync(id, taskItemToUpdateRequest);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        try
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}