using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts;
public interface IEmployeeRepository
{
    void Update(ApplicationUser employee);
    void Create(ApplicationUser employee);
    void Delete(ApplicationUser employee);
    Task<IEnumerable<ApplicationUser>> GetEmployeesAsync(int companyId, bool trackChanges = false);
    Task<ApplicationUser?> GetEmployeeAsync(int companyId, int employeeId, bool trackChanges = false);
}
