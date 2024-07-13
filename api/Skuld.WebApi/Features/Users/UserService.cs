using AutoMapper;
using Skuld.Data.Entities;
using Skuld.Data.UnitOfWork;
using Skuld.WebApi.Exceptions;
using Skuld.WebApi.Features.Users.Dto;
using Skuld.WebApi.Infrastructure.ErrorHandling;
using System.Net;
using System.Threading.Tasks;

namespace Skuld.WebApi.Features.Users;

public interface IUserService
{
	Task<UserResponse> GetUserAsync (long userId);

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

	public async Task<UserResponse> GetUserAsync (long userId)
	{
		var user = await UnitOfWork.UserRepository.TryGetFirstAsync (user => user.UserId == userId);
		if (user is null)
			throw new SkuldException (HttpStatusCode.NotFound, SkuldExceptionType.UserNotFound);

		return _mapper.Map<User, UserResponse> (user);
	}

	public async Task<SkuldResult<UserResponse>> GetUserResultAsync (long userId)
	{
		var dbUser = await UnitOfWork.UserRepository.TryGetOneAsync (user => user.UserId == userId);

		if (dbUser is null)
		{
			return SkuldResult<UserResponse>.Error (HttpStatusCode.NotFound, SkuldExceptionType.UserNotFound, userId);
		}

		var user = _mapper.Map<User, UserResponse> (dbUser);

		return SkuldResult<UserResponse>.Success (user);
	}
}
