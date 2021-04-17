using AutoMapper;
using Skuld.Data.Entities;
using Skuld.Shared.DTO.Enum;
using Skuld.Shared.DTO.Users;

namespace Skuld.Shared.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (Role)src.Role));

            this.CreateMap<CreateUserDTO, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore());
        }
    }
}
