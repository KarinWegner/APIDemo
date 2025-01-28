using AutoMapper;
using Companies.Infrastructure.Data;
using Companies.Presemtation.ControllersForTest;
using Companies.Shared.DTOs;
using Controller.Tests.Extensions;
using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Tests
{
    public class RepoControllerTests
    {
        private RepositoryController sut;
        private Mock<UserManager<ApplicationUser>> userManager;
        private Mock<IUnitOfWork> mockUoW;
        private const string userName = "Kalle";

        public RepoControllerTests()
        {
             mockUoW = new Mock<IUnitOfWork>();

            var mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }));

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            userManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null,null, null);
             sut = new RepositoryController(mockUoW.Object, mapper, userManager.Object);
        }
        [Fact]
        public async Task GetEmployees_ShouldReturnAllEmployees()
        {
            var users = GetUsers();
            mockUoW.Setup(x => x.EmployeeRepository.GetEmployeesAsync(It.IsIn<int>(2,3), It.IsAny<bool>())).ReturnsAsync(users);
            userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new ApplicationUser { UserName = userName });

            sut.SetUserIsAuth(true);

            var result = await sut.GetEmployees(2);
            //var resultType = result.Result as OkObjectResult;

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsType<List<EmployeeDto>>(okObjectResult.Value);
           

            Assert.Equal(items.Count, users.Count);
        }

        [Fact]
        public async Task GetEmployees_ShouldThrowExceptionIfUserNotFound()
        {
             await Assert.ThrowsAsync<ArgumentNullException>(async ()=> await sut.GetEmployees(1));
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
