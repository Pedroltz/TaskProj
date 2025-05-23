
using Xunit;
using TaskApi.Controllers;
using TaskApi.Data;
using TaskApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace TaskApi.Tests
{
    public class UsersControllerTests
    {
        private ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateUser_AddsUserSuccessfully()
        {
            using var context = GetDb();
            var controller = new UsersController(context);

            var user = new User
            {
                Name = "Teste",
                Email = "teste@email.com",
                Password = "senha123"
            };

            var result = await controller.Create(user);
            var created = Assert.IsType<CreatedAtActionResult>(result.Result);

            var createdUser = Assert.IsType<User>(created.Value);
            Assert.Equal(user.Email, createdUser.Email);
        }

        [Fact]
        public async Task GetById_ReturnsUser_IfExists()
        {
            using var context = GetDb();
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Maria",
                Email = "maria@email.com",
                Password = "123"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = new UsersController(context);
            var result = await controller.GetById(user.Id);

            Assert.Equal(user.Id, result.Value!.Id);
        }

        [Fact]
        public async Task DeleteUser_RemovesUser()
        {
            using var context = GetDb();
            var user = new User { Id = Guid.NewGuid(), Name = "Deletar", Email = "x@x.com", Password = "123" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = new UsersController(context);
            var result = await controller.Delete(user.Id);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
