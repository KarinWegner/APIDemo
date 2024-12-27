using AutoMapper;
using Companies.Shared.DTOs;
using Domain.Contracts;
using Domain.Models.Responses;
using Services.Contracts;

namespace Companies.Services;

public class EmployeeService : IEmployeeService
{
    private IUnitOfWork uow;
    private readonly IMapper mapper;

    public EmployeeService(IUnitOfWork uow, IMapper mapper)
    {
        this.uow = uow;
        this.mapper = mapper;
    }

    public async Task<ApiBaseResponse> GetEmployeesAsync(int companyId)
    {
        var companyExists = await uow.CompanyRepository.CompanyExistsAsync(companyId);
        if(!companyExists) return new CompanyNotFoundResponse(companyId);

        var employees = await uow.EmployeeRepository.GetEmployeesAsync(companyId);
        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employees);

        return new ApiOkRepsonse<IEnumerable<EmployeeDto>>(employeesDto);
    }
}