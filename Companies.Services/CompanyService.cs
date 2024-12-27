using AutoMapper;
using Companies.Shared.DTOs;
using Companies.Shared.Request;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
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

    public async Task<(IEnumerable<CompanyDto> companyDtos, MetaData metaData)> GetCompaniesAsync(CompanyRequestParams requestParams, bool trackChanges = false)
    {
        var pagedList = await uow.CompanyRepository.GetCompaniesAsync(requestParams, trackChanges);
        var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(pagedList.Items);
        return (companiesDto, pagedList.MetaData);
    }

    public async Task<CompanyDto> GetCompanyAsync(int id, bool trackChanges = false)
    {
        Company? company = await uow.CompanyRepository.GetCompanyAsync(id);

        if (company == null)
        {
            throw new CompanyNotFoundException(id);
        }

        return mapper.Map<CompanyDto>(company);
    }
}