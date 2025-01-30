using AutoMapper;
using Companies.Infrastructure.Data;
using Companies.Shared.DTOs;
using Companies.Shared.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System.Text.Json;

namespace Companies.Presemtation.Controllers;

[Route("api/simple")]
[ApiController]
[Authorize]
public class SimpleController : ControllerBase
{
    private readonly CompaniesContext context;
    private readonly IMapper mapper;

    public SimpleController(CompaniesContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompany()
    {
        if (User?.Identity?.IsAuthenticated ?? false)
        {
            return Ok("is auth");
        }
        else
        {
        return BadRequest("Is not auth");

        }
    }
    [HttpGet("uniqueroute")]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompany2()
    {
        var companies = await context.Companies.ToListAsync();
        var compDtos = mapper.Map<IEnumerable<CompanyDto>>(companies);
        return Ok(compDtos);
    }
}

