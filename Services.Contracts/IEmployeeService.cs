using Domain.Models.Responses;

namespace Services.Contracts;

public interface IEmployeeService
{
    Task<ApiBaseResponse> GetEmployeesAsync(int companyId);
}
