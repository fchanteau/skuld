using AutoMapper;
using Skuld.Data.Entities;
using Skuld.Data.UnitOfWork;
using Skuld.WebApi.Common.ErrorHandling;
using Skuld.WebApi.Features.Users.Dto;
using System.Net;
using System.Threading.Tasks;

namespace Skuld.WebApi.Features.Users;

public interface IUserService
{
	Task<SkuldResult<UserResponse>> GetUserResultAsync (long userId);
}

public class UserService : BaseService, IUserService
{
	private readonly IMapper _mapper;

	public UserService (IUnitOfWork unitOfWork) : base (unitOfWork)
	{
		var config = new MapperConfiguration (cfg =>
		{
			cfg.AddProfile<UserProfile> ();
		});
		_mapper = new Mapper (config);
	}

	public async Task<SkuldResult<UserResponse>> GetUserResultAsync (long userId)
	{
		var dbUser = await UnitOfWork.UserRepository.TryGetOneAsync (user => user.UserId == userId);

		if (dbUser is null)
		{
			return SkuldResult<UserResponse>.Error (HttpStatusCode.NotFound, SkuldErrorType.UserNotFound, userId);
		}

		var user = _mapper.Map<User, UserResponse> (dbUser);

		return SkuldResult<UserResponse>.Success (user);
	}
}
