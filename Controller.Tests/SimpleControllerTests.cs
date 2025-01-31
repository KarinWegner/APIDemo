using Companies.Presemtation.Controllers;
using Companies.Shared.DTOs;
using Controller.Tests.Extensions;
using Controller.Tests.TextFixtures;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;

namespace Controller.Tests
{
    public class SimpleControllerTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture fixture;

        public SimpleControllerTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }
        [Fact]
        public async void GetCompany_Should_Return400()
        {
            //sut=system under test
            var sut =  fixture.Sut;

            var res=await sut.GetCompany();
            var resultType = res.Result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(resultType);
            Assert.Equal(StatusCodes.Status400BadRequest, resultType.StatusCode);

        }

        [Fact]
        public async Task GetCompany_IfNotAuth_Should_Return400BadRequest()
        {
            //var httpContextMock = new Mock<HttpContext>();
            //httpContextMock.SetupGet(x => x.User.Identity.IsAuthenticated).Returns(false);
            var httpContext = Mock.Of<HttpContext>(x=>x.User.Identity.IsAuthenticated == false);

            var controllerContextMock = new ControllerContext
            {
                HttpContext = httpContext,
            };

            var sut = fixture.Sut;
            sut.ControllerContext = controllerContextMock;

            var res = await sut.GetCompany();
            var resultType = res.Result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(resultType);
            Assert.Equal("Is not auth", resultType.Value);
        }
        [Fact]
        public async Task GetCompany_IfNotAuth_ShouldReturn400BadRequest2()
        {
           // var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
           // mockClaimsPrincipal.SetupGet(x => x.Identity.IsAuthenticated).Returns(false);

            var sut = fixture.Sut;
            sut.SetUserIsAuth(false);
            //sut.ControllerContext = new ControllerContext
            //{
            //    HttpContext = new DefaultHttpContext()
            //    {
            //        User = mockClaimsPrincipal.Object,
            //    }
            //};

            var result= await sut.GetCompany();
            var resultType = result.Result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(resultType);
            Assert.Equal(StatusCodes.Status400BadRequest, resultType.StatusCode);
        }

        [Fact]
        public async Task GetCompany_IfAuth_ShouldReturn200OK()
        {
            var sut = fixture.Sut;
            sut.SetUserIsAuth(true);
         

            var result = await sut.GetCompany();
            var resultType = result.Result as OkObjectResult;

            Assert.IsType<OkObjectResult>(resultType);
        }
        //[Fact]
        //public async Task GetCompany_ShouldReturnExpectedCount()
        //{
        //    var sut = fixture.Sut;
        //    var expectedCount = fixture.Context.Companies.Count();

        //    var result = await sut.GetCompany2();

        //    var resultType = result.Result as OkObjectResult;

        //    var res = Assert.IsType<OkObjectResult>(resultType);
            
        //    Assert.Equal(expectedCount, (IEnumerable<CompanyDto>res.Value).Count();

        //}

    }
}
