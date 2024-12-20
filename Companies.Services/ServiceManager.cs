using AutoMapper;
using Domain.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Services;
public class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> companyService;
    private readonly Lazy<IEmployeeService> employeeService;

    public ICompanyService CompanyService => companyService.Value;
    public IEmployeeService EmployeeService => employeeService.Value;

    public ServiceManager(Lazy<ICompanyService> companyservice, Lazy<IEmployeeService> employeeservice)
    {
        companyService = companyservice;
        employeeService = employeeservice;
    }
}
