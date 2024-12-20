using AutoMapper;
using Companies.Shared.DTOs;
using Domain.Models.Entities;

namespace Companies.Infrastructure.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ForMember(dest => dest.Address,
            opt => opt.MapFrom(src => $"{src.Address}{(string.IsNullOrEmpty(src.Country) ? string.Empty : ", ")}{src.Country}"));

        CreateMap<CompanyCreateDto, Company>();
        CreateMap<CompanyUpdateDto, Company>();


        CreateMap<ApplicationUser, EmployeeDto>().ReverseMap();
        CreateMap<ApplicationUser, EmployeeUpdateDto>().ReverseMap();

        CreateMap<UserForRegistrationDto, ApplicationUser>();
    }
}
