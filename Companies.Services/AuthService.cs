using AutoMapper;
using Companies.Shared.DTOs;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;
        private ApplicationUser? user;

        public AuthService(IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) 
        {
            this.mapper= mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.config = config;
        }

        public async Task<TokenDto> CreateTokenAsync(bool expireTime)
        {
            SigningCredentials signing = GetSigningCredentials();
            IEnumerable<Claim> claims = await GetClaimsAsync();
            JwtSecurityToken tokenOptions = GenerateTokenOptions(signing, claims);

            user!.RefreshToken = GenerateRefreshToken();

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private string? GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber)
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signing, IEnumerable<Claim> claims)
        {
            var jwtSettings = config.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                                            issuer: jwtSettings["Issuer"],
                                            audience: jwtSettings["Audience"],
                                            claims: claims,
                                            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["Expires"])),
                                            signingCredentials: signing
                                            );
            return tokenOptions;
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            ArgumentNullException.ThrowIfNull(nameof(user));
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Age", user.Age.ToString())

                //Add more if needed
            };


            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secretKey = config["secretkey"];
            ArgumentNullException.ThrowIfNull(nameof(secretKey));

            byte[] key = Encoding.UTF8.GetBytes(secretKey);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<IdentityResult> RegisterUserAsync(UserForRegistrationDto registrationDto)
        {
            if (registrationDto is null)   
                throw new ArgumentNullException(nameof(registrationDto));

            var roleExists = await roleManager.RoleExistsAsync(registrationDto.Role);

            if (!roleExists)   
                return IdentityResult.Failed(new IdentityError { Description = "Role does not exist" });

            var user = mapper.Map<ApplicationUser>(registrationDto);

            var result = await userManager.CreateAsync(user, registrationDto.Password!);

            if (result.Succeeded) 
                await userManager.AddToRoleAsync(user, registrationDto.Role);

            return result;
        }

        public async Task<bool> ValidateUserAsync(UserForAuthDto authDto)
        {
            if (authDto is null)
                throw new ArgumentNullException(nameof(authDto));

            user = await userManager.FindByNameAsync(authDto.UserName);

            return user != null && await userManager.CheckPasswordAsync(user, authDto.Password);
        }
    }
}
