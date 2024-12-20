using Companies.Infrastructure.Data;
using Domain.Contracts;

namespace Companies.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CompaniesContext context;
    private readonly Lazy<ICompanyRepository> companyRepository;
    private readonly Lazy<IEmployeeRepository> employeeRepository;

    public ICompanyRepository CompanyRepository => companyRepository.Value;
    public IEmployeeRepository EmployeeRepository => employeeRepository.Value;

    //Add More Repos

    public UnitOfWork(CompaniesContext context, Lazy<ICompanyRepository> companyrepository, Lazy<IEmployeeRepository> employeerepository)
    {
        this.context = context;
        companyRepository = companyrepository;
        employeeRepository = employeerepository;
    }

    public async Task CompleteASync()
    {
        await context.SaveChangesAsync();
    }
}
