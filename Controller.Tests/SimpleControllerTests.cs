using Companies.Presemtation.Controllers;
using Controller.Tests.Extensions;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Controller.Tests
{
    public class SimpleControllerTests
    {
        [Fact]
        public async void GetCompany_Should_Return400()
        {
            //sut=system under test
            var sut = new SimpleController();

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

            var sut = new SimpleController();
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

            var sut = new SimpleController();
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
            var sut = new SimpleController();
            sut.SetUserIsAuth(true);
         

            var result = await sut.GetCompany();
            var resultType = result.Result as OkObjectResult;

            Assert.IsType<OkObjectResult>(resultType);
        }

    }
}
