using Domain.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Companies.Presemtation.Controllers
{    
    public class ApiController : ControllerBase
    {
        [NonAction]
        public ActionResult ProcessError(ApiBaseResponse baseResponse)
        {
            return baseResponse switch
            { 
                ApiNotFoundResponse => NotFound(Results.Problem
                (
                    detail: ((ApiNotFoundResponse)baseResponse).Message,
                     statusCode: StatusCodes.Status404NotFound,
                    title: "Not Found",
                    instance: HttpContext.Request.Path
                )),
                 _=>throw new NotImplementedException()
            };

        }
    }
}
