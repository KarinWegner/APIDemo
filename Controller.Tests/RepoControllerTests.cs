using Companies.Presemtation.ControllersForTest;
using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Tests
{
    public class RepoControllerTests
    {
        private Mock<IEmployeeRepository> mockRepo;
        private RepositoryController sut;

        public RepoControllerTests()
        {
             mockRepo = new Mock<IEmployeeRepository>();
             sut = new RepositoryController(mockRepo.Object);
        }
        [Fact]
        public async Task GetEmployees_ShouldReturnAllEmployees()
        {
            var users = GetUsers();

            mockRepo.Setup(x => x.GetEmployeesAsync(It.IsIn<int>(2,3), It.IsAny<bool>())).ReturnsAsync(users);

            var result = await sut.GetEmployees(2);
            //var resultType = result.Result as OkObjectResult;

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsType<List<ApplicationUser>>(okObjectResult.Value);
           

            Assert.Equal(items.Count, users.Count);
        }

        private List<ApplicationUser> GetUsers()
        {
            return new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id="1",
                    Name="Kalle",
                    Age=12,
                    UserName = "Kalle"
                },
                new ApplicationUser
                {
                    Id="2",
                    Name="Kalle",
                    Age=12,
                    UserName="Kalle"
                }
            };
        }
    }
}
