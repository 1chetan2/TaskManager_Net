using JwtApi.DTOs;
using JwtApi.Models;
using JwtApi.Data;   
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskItemController(AppDbContext context)
        {
            _context = context;
        }
        //[HttpGet]
        //public IActionResult GetAllTasks()
        //{
        //    var tasks = _context.TaskItems.ToList();
        //    return Ok(tasks);
        //}

        [HttpGet]
        public IActionResult GetAllTasks()
        {
            var tasks = _context.TaskItems
                .Include(t => t.TaskStatus)
                .Select(t => new TaskItemDto
                {
                    Date = t.Date,
                    TaskName = t.TaskName,
                    Task = t.Task,
                    Hours = t.Hours,
                    StatusValue = t.TaskStatus.StatusValue
                })
                .ToList();

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItemDto dto)
        {
            var task = new TaskItem
            {
                Date = dto.Date,
                TaskName = dto.TaskName,
                Task = dto.Task,
                Hours = dto.Hours
            };

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();

            var status = new TaskChild
            {
                TaskItemId = task.Id,
                StatusValue = dto.StatusValue
            };

            _context.TaskChilds.Add(status);
            await _context.SaveChangesAsync();

            return Ok("Task created successfully");
        }

  

    }
}
