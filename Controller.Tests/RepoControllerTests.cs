using AutoMapper;
using Companies.Infrastructure.Data;
using Companies.Presemtation.ControllersForTest;
using Companies.Shared.DTOs;
using Controller.Tests.Extensions;
using Controller.Tests.TextFixtures;
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
    public class RepoControllerTests : IClassFixture<RepoControllerFixture>
    {
       
        private const string userName = "Kalle";
        private readonly RepoControllerFixture fixture;

        public RepoControllerTests(RepoControllerFixture fixture)
        {
            this.fixture = fixture;
        }

     
        [Fact]
        public async Task GetEmployees_ShouldReturnAllEmployees()
        {
            var users = fixture.GetUsers();
            var dtos = fixture.Mapper.Map<IEnumerable<EmployeeDto>>(users);
            ApiBaseResponse baseResponse = new ApiOkRepsonse<IEnumerable<EmployeeDto>>(dtos);

            fixture.ServiceManagerMock.Setup(x => x.EmployeeService.GetEmployeesAsync(It.IsIn<int>(2,3))).ReturnsAsync(baseResponse);
            fixture.UserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new ApplicationUser { UserName = userName });

            fixture.Sut.SetUserIsAuth(true);

            var result = await fixture.Sut.GetEmployees(2);
            //var resultType = result.Result as OkObjectResult;

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsType<List<EmployeeDto>>(okObjectResult.Value);
           

            Assert.Equal(items.Count, users.Count);
        }

        [Fact]
        public async Task GetEmployees_ShouldThrowExceptionIfUserNotFound()
        {
            //kollar hur många gånger en metod körs
            fixture.ServiceManagerMock.Verify(x => x.EmployeeService.GetEmployeesAsync(2), Times.Never());


             await Assert.ThrowsAsync<ArgumentNullException>(async ()=> await fixture.Sut.GetEmployees(2));
        }

        
    }
}
