using AutoMapper;
using Companies.Shared.DTOs;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) 
        {
            _mapper=mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task<string> CreateTokenAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> RegisterUserAsync(UserForRegistrationDto registrationDto)
        {
            if (registrationDto is null)   
                throw new ArgumentNullException(nameof(registrationDto));

            var roleExists = await _roleManager.RoleExistsAsync(registrationDto.Role);

            if (!roleExists)   
                return IdentityResult.Failed(new IdentityError { Description = "Role does not exist" });

            var user = _mapper.Map<ApplicationUser>(registrationDto);

            var result = await _userManager.CreateAsync(user, registrationDto.Password!);

            if (result.Succeeded) 
                await _userManager.AddToRoleAsync(user, registrationDto.Role);

            return result;
        }

        public async Task<bool> ValidateUserAsync(UserForAuthDto authDto)
        {
            if (authDto is null)
                throw new ArgumentNullException(nameof(authDto));

            var user = await _userManager.FindByNameAsync(authDto.UserName);

            return user != null && await _userManager.CheckPasswordAsync(user, authDto.Password);
        }
    }
}
