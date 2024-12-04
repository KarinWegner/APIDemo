﻿using Companies.Infrastructure.Data;
using Domain.Contracts;

namespace Companies.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CompaniesContext context;

    public ICompanyRepository CompanyRepository { get; }
    public IEmployeeRepository EmployeeRepository { get; }

    //Add More Repos

    public UnitOfWork(CompaniesContext context)
    {
        this.context = context;
        CompanyRepository = new CompanyRepository(context);
        EmployeeRepository = new EmployeeRepository(context);
    }

    public async Task CompleteASync()
    {
        await context.SaveChangesAsync();
    }
}
