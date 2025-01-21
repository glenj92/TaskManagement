using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.DTOs;

namespace TaskManagement.Controllers
{
    // Controller for Task Management
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _context.Taskreas.Include(t => t.AssignedUser).ToListAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _context.Taskreas.Include(t => t.AssignedUser).FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] Taskrea taskrea)
        {
            taskrea.CreatedAt = DateTime.UtcNow;
            taskrea.UpdatedAt = DateTime.UtcNow;

            _context.Taskreas.Add(taskrea);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = taskrea.Id }, taskrea);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Taskrea updatedTask)
        {
            var task = await _context.Taskreas.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.Status = updatedTask.Status;
            task.AssignedUserId = updatedTask.AssignedUserId;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Taskreas.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Taskreas.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}