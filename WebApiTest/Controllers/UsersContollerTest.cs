using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;

namespace WebApiTest.Controllers
{
    public class UsersContollerTest
    {
        [Fact]
        public async Task Get_OkWithAllUsers()
        {
            //Arrange
            var service = new Mock<ICrudService<User>>();
            var expectedList = new Fixture().CreateMany<User>();
            service.Setup(x => x.ReadAsync()).ReturnsAsync(expectedList);

            var controller = new UsersController(service.Object);

            //Act
            var result = await controller.Get();

            //Arrange
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var resultList = Assert.IsAssignableFrom<IEnumerable<User>>(actionResult.Value);
            Assert.Equal(expectedList, resultList);
        }

        [Fact]
        public async Task Get_ExistingId_OkWithUser()
        {
            //Arrange
            var service = new Mock<ICrudService<User>>();
            var expectedUser = new Fixture().Create<User>();
            service.Setup(x => x.ReadAsync(expectedUser.Id)).ReturnsAsync(expectedUser);

            var controller = new UsersController(service.Object);

            //Act
            var result = await controller.Get(expectedUser.Id);

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var resultUser = Assert.IsAssignableFrom<User>(actionResult.Value);
            Assert.Equal(expectedUser, resultUser);
        }

        [Fact]
        public Task Get_NotExistingId_NotFound()
        {

            return ReturnsNotFound((controller, id) => controller.Get(id));
        }
        [Fact]
        public Task Delete_NotExistingId_NotFound()
        {
            return ReturnsNotFound((controller, id) => controller.Delete(id));
        }
        [Fact]
        public Task Put_NotExistingId_NotFound()
        {
            var user = new Fixture().Create<User>();
            return ReturnsNotFound((controller, id) => controller.Put(id, user));
        }

        private async Task ReturnsNotFound(Func<UsersController, int, Task<IActionResult>> func)
        {
            //Arrange
            int id = new Fixture().Create<int>();
            var service = new Mock<ICrudService<User>>();

            var controller = new UsersController(service.Object);

            //Act
            var result = await func(controller, id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ExistingId_NoContent()
        {
            //Arrange
            var service = new Mock<ICrudService<User>>();
            var expectedUser = new Fixture().Create<User>();

            //do weryfikacji?
            var sequence = new MockSequence();
            service.InSequence(sequence).Setup(x => x.ReadAsync(expectedUser.Id)).ReturnsAsync(expectedUser);
            service.InSequence(sequence).Setup(x => x.DeleteAsync(expectedUser.Id));

            var controller = new UsersController(service.Object);

            //Act
            var result = await controller.Delete(expectedUser.Id);

            //Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            service.Verify(x => x.ReadAsync(expectedUser.Id));
            service.Verify(x => x.DeleteAsync(expectedUser.Id), Times.Once);
        }
    }
}
