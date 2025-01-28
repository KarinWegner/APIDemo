using AutoMapper;
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
        private readonly IMapper mapper;

        public RepositoryController(IEmployeeRepository employeeRepo, IMapper mapper)
        {
            this.employeeRepo = employeeRepo;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees(int id)
        {
            var employees = await employeeRepo.GetEmployeesAsync(id);

            var dtos = mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(dtos);
        }
    }
}
