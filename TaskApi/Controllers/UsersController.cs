using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.Models;
using TaskApi.Dtos;
using TaskApi.Services;
using TaskApi.Models.DTOs;

namespace TaskApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly FirebaseAuthService _firebase;

        public UsersController(ApplicationDbContext context, FirebaseAuthService firebase)
        {
            _context = context;
            _firebase = firebase;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
            => await _context.Users
                             .Include(u => u.Tasks)
                             .ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            var user = await _context.Users
                                     .Include(u => u.Tasks)
                                     .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] CreateUserDto userDto)
        {
            var firebaseUid = await _firebase.CreateUserAsync(userDto.Email, userDto.Password);

            var user = new User
            {
                Id = firebaseUid,
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] User user)
        {
            if (id != user.Id)
                return BadRequest();

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return NotFound();

            // Atualiza no Firebase
            await _firebase.UpdateUserAsync(
                existingUser.Id,
                email: user.Email,
                password: user.Password
            );

            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.Name = user.Name;

            _context.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            await _firebase.DeleteUserAsync(user.Id);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{userId}/tasks")]
        public async Task<ActionResult<TaskItem>> CreateTaskForUser(string userId, [FromBody] CreateTaskDto taskDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound($"User {userId} not found.");

            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate ?? DateTime.Now,
                Priority = taskDto.Priority,
                Status = taskDto.Status,
                UserId = userId
            };

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                actionName: "GetById",
                controllerName: "TaskItems",
                routeValues: new { id = task.Id },
                value: task
            );
        }

        [HttpPut("{userId}/tasks/{taskId}")]
        public async Task<IActionResult> UpdateTaskForUser(string userId, Guid taskId, [FromBody] CreateTaskDto taskDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound($"User {userId} not found.");

            var task = await _context.TaskItems
                                     .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
            if (task == null)
                return NotFound($"Task {taskId} not found for user {userId}.");

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.DueDate = taskDto.DueDate ?? DateTime.Now;
            task.Priority = taskDto.Priority;
            task.Status = taskDto.Status;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{userId}/tasks/{taskId}")]
        public async Task<IActionResult> DeleteTaskForUser(string userId, Guid taskId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound($"User {userId} not found.");

            var task = await _context.TaskItems
                                     .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
            if (task == null)
                return NotFound($"Task {taskId} not found for user {userId}.");

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
