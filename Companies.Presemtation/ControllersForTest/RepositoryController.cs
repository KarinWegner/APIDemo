using AutoMapper;
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
    public class RepositoryController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public RepositoryController(IUnitOfWork uow, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this.uow = uow;
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

            var employees = await uow.EmployeeRepository.GetEmployeesAsync(id);

            var dtos = mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(dtos);
        }
    }
}
