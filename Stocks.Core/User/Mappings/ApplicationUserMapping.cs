using AutoMapper;

using Stocks.Core.DTO;
using Stocks.Core.Identity;

namespace Stocks.Core.User.Mappings
{
    /* The class `ApplicationUserMapping` maps properties from a `RegisterDTO` object to an
    `ApplicationUser` object. */
    public class ApplicationUserMapping : Profile
    {
        public ApplicationUserMapping()
        {
            CreateMap<RegisterDTO, ApplicationUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(
                    src => src.Email
                ))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(
                    src => src.PhoneNumber
                ))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(
                    src => src.Email
                ))
                .ForMember(dest => dest.PersonName, opt => opt.MapFrom(
                    src => src.PersonName
                ));
        }
    }
}

