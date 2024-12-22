using AutoMapper;
using Companies.Shared.DTOs;
using Domain.Models.Configuration;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
        private readonly JwtConfiguration jwtConfig;
       // private readonly IConfiguration config;
        private ApplicationUser? user;

        public AuthService(IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,IOptions<JwtConfiguration> jwtConfig/*, IConfiguration config*/) 
        {
            this.mapper= mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.jwtConfig = jwtConfig.Value;
           // this.config = config;
        }

        public async Task<TokenDto> CreateTokenAsync(bool expireTime)
        {
            ArgumentNullException.ThrowIfNull(nameof(user));

            SigningCredentials signing = GetSigningCredentials();
            IEnumerable<Claim> claims = await GetClaimsAsync();
            JwtSecurityToken tokenOptions = GenerateTokenOptions(signing, claims);

            user!.RefreshToken = GenerateRefreshToken();

            if (expireTime)
                user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

            var res = await userManager.UpdateAsync(user);

            //Validate res
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDto(accessToken, user.RefreshToken!);
        }

        private string? GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signing, IEnumerable<Claim> claims)
        {
            //var jwtSettings = config.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                                            issuer: jwtConfig.Issuer,
                                            audience: jwtConfig.Audience,
                                            claims: claims,
                                            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtConfig.Expires)),
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
            //var secretKey = config["secretkey"];
            //ArgumentNullException.ThrowIfNull(nameof(jwtConfig.SecretKey));

            byte[] key = Encoding.UTF8.GetBytes(jwtConfig.SecretKey);
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

        public async Task<TokenDto> RefreshTokenAsync(TokenDto token)
        {
            ClaimsPrincipal principal = GetPrincipalFromExpiredToken(token.AccessToken);

            ApplicationUser? user = await userManager.FindByNameAsync(principal.Identity?.Name!);
            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiration <= DateTime.UtcNow)
            {
                //ToDo: Handle with middleware and custom exeption class
                throw new ArgumentException("The TokenDto has some invalid values");
            }

            this.user = user;

            return await CreateTokenAsync(expireTime: false);

        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            //var jwtSettings = config.GetSection("JwtSettings");
            //ArgumentNullException.ThrowIfNull(nameof(jwtSettings));

            //var secretKey = config["secretkey"];
            //ArgumentNullException.ThrowIfNull(nameof(secretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return claimsPrincipal;
        }
    }
}
