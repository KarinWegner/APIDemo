using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts.ContractsForTests
{
    public interface IUserService
    {
        Task<ApplicationUser?> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        Task<bool> IsInRoleAsync(ApplicationUser user, string role);
    }
}
