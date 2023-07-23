using AutoMapper;
using Skuld.Data.Entities;
using Skuld.WebApi.Features.Users.Dto;

namespace Skuld.WebApi.Features.Users;

public class UserProfile : Profile
{
	public UserProfile ()
	{
		CreateMap<User, UserResponse> ()
			.ForMember (dest => dest.Role, opt => opt.MapFrom (src => (Role)src.Role))
			.ReverseMap ();
	}
}
