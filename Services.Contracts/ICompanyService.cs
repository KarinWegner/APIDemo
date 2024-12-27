using Companies.Shared.DTOs;
using Companies.Shared.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts;
public interface ICompanyService
{
    Task<(IEnumerable<CompanyDto> companyDtos, MetaData metaData)> GetCompaniesAsync(CompanyRequestParams requestParams, bool trackChanges = false);
    Task<CompanyDto> GetCompanyAsync(int id, bool trackChanges = false);
}
