using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Skuld.Data.Entities;
using Skuld.Data.UnitOfWork;
using Skuld.Shared.DTO.Users;
using Skuld.Shared.Exceptions;
using Skuld.Shared.Helpers;
using Skuld.Shared.Infrastructure.Configuration.Options;
using Skuld.Shared.Infrastructure.Constants;
using Skuld.Shared.MappingProfiles;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Skuld.Shared.Services
{
    public class UserService : BaseService
    {
        private readonly JwtOptions jwtOptions;
        
        #region Constructor

        public UserService(UnitOfWork unitOfWork, IOptions<JwtOptions> jwtOptions) : base(unitOfWork)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
            });

            config.AssertConfigurationIsValid();

            this.Mapper = new Mapper(config);
            this.jwtOptions = jwtOptions.Value;
        }

        #endregion

        #region Public methods

        public async Task<UserDTO> CreateUserAsync(CreateUserDTO model)
        {
            // check if user already exist with the email
            var userAlreadyExist = await this._unitOfWork.UserRepository.AnyAsync(x => x.Email.Equals(model.Email));
            if (userAlreadyExist)
                throw new SkuldException(HttpStatusCode.BadRequest, SkuldExceptionType.UserAlreadyExist, model.Email);

            // secured password
            model.Password = Convert.ToBase64String(Encoding.ASCII.GetBytes(model.Password));

            var user = this.Mapper.Map<CreateUserDTO, User>(model);

            // insert user
            this._unitOfWork.UserRepository.Insert(user);
            this._unitOfWork.Save();

            // create refresh token for this user
            this._unitOfWork.RefreshTokenRepository.Insert(TokenHelper.BuildRefreshToken(user, DateTime.Now.AddDays(7)));
            this._unitOfWork.Save();

            var userCreated = await this._unitOfWork.UserRepository.TryGetByIdAsync(user.UserId);

            return this.Mapper.Map<User, UserDTO>(userCreated);
        }

        public async Task<(string, string)> LoginAsync(UserLoginDTO model)
        {
            var cryptedPassword = Convert.ToBase64String(Encoding.ASCII.GetBytes(model.Password));

            var user = await this._unitOfWork.UserRepository.TryGetOneAsync(filter: x => x.Email.Equals(model.Email) && x.Password.Equals(cryptedPassword));
            if (user == null)
                throw new SkuldException(HttpStatusCode.BadRequest, SkuldExceptionType.UserLoginFailed);

            string token = TokenHelper.CreateToken(user, this.jwtOptions);

            var refreshToken = await this._unitOfWork.RefreshTokenRepository.TryGetFirstAsync(
                filter: x => x.UserId == user.UserId,
                orderBy: x => x.OrderByDescending(rt => rt.ExpiredDate));

            if (refreshToken == null)
            {
                throw new SkuldException(HttpStatusCode.BadRequest, SkuldExceptionType.UserLoginFailed);
            }

            if (refreshToken.ExpiredDate < DateTime.Now)
            {
                refreshToken = TokenHelper.BuildRefreshToken(user, DateTime.Now.AddDays(7));
                this._unitOfWork.RefreshTokenRepository.Insert(refreshToken);
                this._unitOfWork.Save();
            }

            return (token, refreshToken.Value);
        }

        public async Task<UserDTO> GetUserAsync(decimal userId)
        {
            var user = await this._unitOfWork.UserRepository.TryGetByIdAsync(userId);
            if (user == null)
                throw new SkuldException(HttpStatusCode.NotFound, SkuldExceptionType.UserNotFound);

            return this.Mapper.Map<User, UserDTO>(user);
        }

        public async Task<string> ValidRefreshToken(decimal userId, string refreshToken)
        {
            var response = await this._unitOfWork.RefreshTokenRepository.TryGetOneAsync(filter: x => x.Value == refreshToken && x.UserId == userId);
            if (response == null ||
                response.ExpiredDate < DateTime.Now)
            {
                throw new SkuldException(HttpStatusCode.BadRequest, SkuldExceptionType.RefreshTokenInvalid);
            }

            var user = await this._unitOfWork.UserRepository.TryGetByIdAsync(userId);

            return TokenHelper.CreateToken(user, jwtOptions);
        }

        #endregion
    }
}
