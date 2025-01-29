using AutoMapper;
using Companies.Presemtation.Controllers;
using Companies.Shared.DTOs;
using Companies.Shared.Request;
using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Security.Claims;
using System.Text.Json;

namespace Companies.Presemtation.ControllersForTest
{
    [Route("api/repo/{id}")]
    [ApiController]
    public class RepositoryController : ApiControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IServiceManager serviceManager;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public RepositoryController(IServiceManager serviceManager,IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this.serviceManager = serviceManager;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees(int id)
        {
            //var userId2 = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.NameIdentifier)?.Value;

            //if (string.IsNullOrEmpty(userId2)) throw new NullReferenceException(nameof(userId2));
            var user = await userManager.GetUserAsync(User);
            if(user is null) throw new ArgumentNullException(nameof(user));

            var response = await serviceManager.EmployeeService.GetEmployeesAsync(id);


            return response.Success ? 
                Ok(response.GetOkResult<IEnumerable<EmployeeDto>>()) :
                ProcessError(response);
        }
    }
}
