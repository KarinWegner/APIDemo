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
    private readonly Lazy<IAuthService> authService;
    private readonly Lazy<IUserService> userService;

    public ICompanyService CompanyService => companyService.Value;
    public IEmployeeService EmployeeService => employeeService.Value;
    public IAuthService AuthService => authService.Value;
    public IUserService UserService => userService.Value;

    public ServiceManager(Lazy<ICompanyService> companyservice, Lazy<IEmployeeService> employeeservice, Lazy<IAuthService> authservice, Lazy<IUserService> userservice)
    {
        companyService = companyservice;
        employeeService = employeeservice;
        authService = authservice;
        userService = userservice;
    }
}
