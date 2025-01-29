using AutoMapper;
using Companies.Infrastructure.Data;
using Companies.Presemtation.ControllersForTest;
using Companies.Shared.DTOs;
using Controller.Tests.Extensions;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Contracts;
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
        private Mock<IServiceManager> serviceManagerMock;
        private Mapper mapper;
        private const string userName = "Kalle";

        public RepoControllerTests()
        {
            serviceManagerMock = new Mock<IServiceManager>();
            mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }));

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            userManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null,null, null);
             sut = new RepositoryController(serviceManagerMock.Object, mapper, userManager.Object);
        }

        //[Fact]
        //public void AutoMapper_Configuration_ShouldBeValid()
        //{
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.AddProfile<AutoMapperProfile>();
        //    });

        //    config.AssertConfigurationIsValid();
        //}

        [Fact]
        public async Task GetEmployees_ShouldReturnAllEmployees()
        {
            var users = GetUsers();
            var dtos = mapper.Map<IEnumerable<EmployeeDto>>(users);
            ApiBaseResponse baseResponse = new ApiOkRepsonse<IEnumerable<EmployeeDto>>(dtos);

            serviceManagerMock.Setup(x => x.EmployeeService.GetEmployeesAsync(It.IsIn<int>(2,3))).ReturnsAsync(baseResponse);
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
