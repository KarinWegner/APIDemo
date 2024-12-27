using Companies.Infrastructure.Data;
using Companies.Shared.Request;
using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Companies.Infrastructure.Repositories;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{

    public CompanyRepository(CompaniesContext context) : base(context){}

    public async Task<Company?> GetCompanyAsync(int id, bool trackChanges = false)
    {
        return await FindByCondition(c => c.Id.Equals(id), trackChanges).FirstOrDefaultAsync();
    }

    public async Task<PagedList<Company>> GetCompaniesAsync(CompanyRequestParams requestParams, bool trackChanges = false)
    {
        var companies = requestParams.IncludeEmployees ?  FindAll(trackChanges).Include(c => c.Employees) :
                                                          FindAll(trackChanges);

        return await PagedList<Company>.CreateAsync(companies, requestParams.PageNumber, requestParams.PageSize);
    }

    public async Task<bool> CompanyExistsAsync(int id)
    {
        return await Context.Companies.AnyAsync(c => c.Id == id);
    }
}
