using AutoMapper;
using System.Diagnostics.Metrics;
using webapbackend.Dto;
using webapbackend.Models;
namespace webapbackend.Repository
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Appointment, AppointmentDto>()
                
                .ForMember(dest => dest.ProductName, opt => opt.Ignore());

            CreateMap<AppointmentDto, Appointment>()
                .ForMember(dest => dest.Category, opt => opt.Ignore()) // İsteğe bağlı
                .ForMember(dest => dest.Product, opt => opt.Ignore());  // İsteğe bağlı

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}