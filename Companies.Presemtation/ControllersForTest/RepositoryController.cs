using Companies.Shared.DTOs;
using Companies.Shared.Request;
using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Text.Json;

namespace Companies.Presemtation.ControllersForTest
{
    [Route("api/repo/{id}")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepo;

        public RepositoryController(IEmployeeRepository employeeRepo)
        {
            this.employeeRepo = employeeRepo;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetEmployees(int id)
        {
            var employees = await employeeRepo.GetEmployeesAsync(id);
            return Ok(employees);
        }
    }
}
