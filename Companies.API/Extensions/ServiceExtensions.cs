﻿using System.Security.Cryptography.Xml;
using System.Text;
using Companies.Infrastructure.Repositories;
using Companies.Services;
using Companies.Services.ServiceForTest;
using Domain.Contracts;
using Domain.Models.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Contracts;
using Services.Contracts.ContractsForTests;

namespace Companies.API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(builder =>
        {
            builder.AddPolicy("AllowAll", p =>
                p.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                );
        });
    }
    public static void ConfigureServiceLayerServices(this IServiceCollection services)
    {

        services.AddScoped<IServiceManager, ServiceManager>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
       // services.AddScoped(provider => new Lazy<ICompanyService>(() => provider.GetRequiredService<ICompanyService>()));
        services.AddLazy<IEmployeeService>();
        services.AddLazy<ICompanyService>();
        services.AddLazy<IAuthService>();
    }
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddLazy<IEmployeeRepository>();
        services.AddLazy<ICompanyRepository>();
    }

    public static void ConfigureOpenApi(this IServiceCollection services) =>
        services.AddEndpointsApiExplorer()
        .AddSwaggerGen(setup =>
        {
            setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Place to add JWT with Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference= new OpenApiReference
                        {
                            Id="Bearer",
                            Type=ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = configuration["secretkey"];
        ArgumentNullException.ThrowIfNull(nameof(secretKey));

        var jwtSettings = configuration.GetSection("JwtSettings");
        ArgumentNullException.ThrowIfNull(nameof(jwtSettings));

        var jwtConfig = new JwtConfiguration();
        jwtSettings.Bind(jwtConfig);

        services.Configure<JwtConfiguration>(options =>
        {
            options.Issuer = jwtConfig.Issuer;
            options.Audience = jwtConfig.Audience;
            options.Expires = jwtConfig.Expires;
            options.SecretKey = secretKey;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
                .AddJwtBearer(options =>
                {

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
                    };
                });

    }
}
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLazy<TService>(this IServiceCollection services) where TService : class
    {
        return services.AddScoped(provider => new Lazy<TService>(() => provider.GetRequiredService<TService>()));
    }
}
