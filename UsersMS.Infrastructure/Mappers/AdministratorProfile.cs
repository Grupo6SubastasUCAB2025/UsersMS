using AutoMapper;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UsersMS.Infrastructure.Mappers
{
    public class AdministratorProfile : Profile
    {
        public AdministratorProfile()
        {
            CreateMap<Administrator, AdministratorDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.Value))
                .ForMember(dest => dest.Cedula, opt => opt.MapFrom(src => src.Cedula.Value))
                .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token != null ? src.Token.Value : null))
                .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => src.IsVerified));
        }
    }
}
