using Companies.Presemtation.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var resultType = res.Result as OkResult;

            Assert.IsType<OkResult>(resultType);
            Assert.Equal(StatusCodes.Status200OK, resultType.StatusCode);

        }
    }
}