

using Companies.Shared.Request;
using Domain.Models.Entities;

namespace Domain.Contracts;
public interface ICompanyRepository
{
    void Update(Company company);
    void Create(Company company);
    void Delete(Company company);
    Task<PagedList<Company>> GetCompaniesAsync(CompanyRequestParams requestParams, bool trackChanges = false);
    Task<Company?> GetCompanyAsync(int id, bool trackChanges = false);
    Task<bool> CompanyExistsAsync(int id);
}