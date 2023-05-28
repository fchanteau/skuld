using AutoMapper;
using Skuld.Data.Entities;
using Skuld.WebApi.Dto.Enum;
using Skuld.WebApi.Dto.Users;

namespace Skuld.WebApi.MappingProfiles
{
	public class UserProfile : Profile
	{
		public UserProfile ()
		{
			this.CreateMap<User, UserResponse> ()
				.ForMember (dest => dest.Role, opt => opt.MapFrom (src => (Role)src.Role))
				.ReverseMap ();

			this.CreateMap<AddUserPayload, User> ()
				.ForMember (dest => dest.UserId, opt => opt.Ignore ())
				.ForMember (dest => dest.Role, opt => opt.Ignore ())
				.ForMember (dest => dest.RefreshTokens, opt => opt.Ignore ())
				.ForMember (dest => dest.Passwords, opt => opt.Ignore ());
		}
	}
}
