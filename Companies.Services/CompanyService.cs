using AutoMapper;
using Companies.Shared.DTOs;
using Domain.Contracts;
using Domain.Models.Entities;
using Services.Contracts;

namespace Companies.Services;

public class CompanyService : ICompanyService
{
    private IUnitOfWork uow;
    private readonly IMapper mapper;

    public CompanyService(IUnitOfWork uow, IMapper mapper)
    {
        this.uow = uow;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<CompanyDto>> GetCompaniesAsync(bool includeEmployees, bool trackChanges = false)
    {
        return mapper.Map<IEnumerable<CompanyDto>>(await uow.CompanyRepository.GetCompaniesAsync(includeEmployees, trackChanges));
    }

    public async Task<CompanyDto> GetCompanyAsync(int id, bool trackChanges = false)
    {
        Company? company = await uow.CompanyRepository.GetCompanyAsync(id);

        if (company == null)
        {
            //ToDo: Fix later
        }

        return mapper.Map<CompanyDto>(company);
    }
}