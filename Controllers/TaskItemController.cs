using JwtApi.DTOs;
using JwtApi.Models;
using JwtApi.Data;   // IMPORTANT
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
