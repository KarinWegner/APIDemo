using Companies.Presemtation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Controller.Tests
{
    public class SimpleControllerTests
    {
        [Fact]
        public async void GetCompany_Should_Return200OK()
        {
            //sut=system under test
            var sut = new SimpleController();

            var res=await sut.GetCompany();
            var resultType = res.Result as OkObjectResult;

            Assert.IsType<OkObjectResult>(resultType);
            Assert.Equal(StatusCodes.Status200OK, resultType.StatusCode);

        }

        [Fact]
        public async Task GetCompany_IfNotAuth_Should_Return400BadRequest()
        {
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(x => x.User.Identity.IsAuthenticated).Returns(false);

            var controllerContextMock = new ControllerContext
            {
                HttpContext = httpContextMock.Object,
            };

            var sut = new SimpleController();
            sut.ControllerContext = controllerContextMock;

            var res = await sut.GetCompany();
            var resultType = res.Result as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(resultType);
            Assert.Equal("Is not auth", resultType.Value);
        }
    }
}