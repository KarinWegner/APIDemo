using System.ComponentModel.DataAnnotations;

namespace Companies.Shared.DTOs;

public record CompanyDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Address { get; init; }
    public IEnumerable<EmployeeDto>? Employees { get; init; }

}

