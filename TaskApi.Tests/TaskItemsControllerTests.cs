
using Xunit;
using Microsoft.EntityFrameworkCore;
using TaskApi.Controllers;
using TaskApi.Data;
using TaskApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TaskApi.Tests
{
    public class TaskItemsControllerTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAll_ReturnsAllTasks()
        {
            using var context = GetInMemoryDbContext();
            context.TaskItems.Add(new TaskItem { Id = Guid.NewGuid(), Title = "Tarefa 1", UserId = Guid.NewGuid() });
            context.TaskItems.Add(new TaskItem { Id = Guid.NewGuid(), Title = "Tarefa 2", UserId = Guid.NewGuid() });
            await context.SaveChangesAsync();

            var controller = new TaskItemsController(context);

            var result = await controller.GetAll();

            var okResult = Assert.IsType<ActionResult<IEnumerable<TaskItem>>>(result);
            var tasks = Assert.IsType<List<TaskItem>>(okResult.Value);
            Assert.Equal(2, tasks.Count);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNotExists()
        {
            using var context = GetInMemoryDbContext();
            var controller = new TaskItemsController(context);

            var result = await controller.GetById(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateTask_ReturnsCreatedTask()
        {
            using var context = GetInMemoryDbContext();
            var controller = new TaskItemsController(context);
            var task = new TaskItem { Title = "Nova Tarefa", UserId = Guid.NewGuid() };

            var result = await controller.Create(task);
            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdTask = Assert.IsType<TaskItem>(created.Value);
            Assert.Equal(task.Title, createdTask.Title);
        }
    }
}
