using AutoMapper;
using Skuld.Data.Entities;
using Skuld.WebApi.Features.Auth.Dto;

namespace Skuld.WebApi.Features.Auth;

public class AuthProfile : Profile
{
	public AuthProfile ()
	{
		CreateMap<AddUserPayload, User> ()
			.ForMember (dest => dest.UserId, opt => opt.Ignore ())
			.ForMember (dest => dest.Role, opt => opt.Ignore ())
			.ForMember (dest => dest.RefreshTokens, opt => opt.Ignore ())
			.ForMember (dest => dest.Passwords, opt => opt.Ignore ());
	}
}
