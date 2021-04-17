using AutoMapper;
using Skuld.Shared.DTO.Users;
using Skuld.WebApi.Features.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skuld.WebApi.Infrastructure.MappingProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<UserDTO, UserGetModel>();
            this.CreateMap<UserPostModel, CreateUserDTO>();
            this.CreateMap<UserPostLoginModel, UserLoginDTO>();
        }
    }
}
