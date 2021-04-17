using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Skuld.Data.Entities;
using Skuld.Data.UnitOfWork;
using Skuld.Shared.DTO.Users;
using Skuld.Shared.Exceptions;
using Skuld.Shared.Infrastructure.Configuration.Options;
using Skuld.Shared.Infrastructure.Constants;
using Skuld.Shared.MappingProfiles;
using System;
using System.IdentityModel.Tokens.Jwt;
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

            this._mapper = new Mapper(config);
            this.jwtOptions = jwtOptions.Value;
        }

        #endregion

        #region Public methods

        public async Task<UserDTO> CreateUserAsync(CreateUserDTO model)
        {
            // check if user already exist with the email
            var userAlreadyExist = await this._unitOfWork.UserRepository.AnyAsync(x => x.Email.Equals(model.Email));
            if (userAlreadyExist)
                throw new SkuldException(System.Net.HttpStatusCode.BadRequest, SkuldExceptionType.UserAlreadyExist, model.Email);

            // secured password
            model.Password = Convert.ToBase64String(Encoding.ASCII.GetBytes(model.Password));

            var user = this._mapper.Map<CreateUserDTO, User>(model);

            this._unitOfWork.UserRepository.Insert(user);
            this._unitOfWork.Save();

            var userCreated = await this._unitOfWork.UserRepository.TryGetByIdAsync(user.UserId);

            return this._mapper.Map<User, UserDTO>(userCreated);
        }

        public async Task<string> LoginAsync(UserLoginDTO model)
        {
            var cryptedPassword = Convert.ToBase64String(Encoding.ASCII.GetBytes(model.Password));

            var user = await this._unitOfWork.UserRepository.TryGetOneAsync(filter: x => x.Email.Equals(model.Email) && x.Password.Equals(cryptedPassword));
            if (user == null)
                throw new SkuldException(HttpStatusCode.BadRequest, SkuldExceptionType.UserLoginFailed);

            string token = this.CreateToken(user);

            return token;
        }

        public async Task<UserDTO> GetUserAsync(decimal userId)
        {
            var user = await this._unitOfWork.UserRepository.TryGetByIdAsync(userId);
            if (user == null)
                throw new SkuldException(HttpStatusCode.NotFound, SkuldExceptionType.UserNotFound);

            return this._mapper.Map<User, UserDTO>(user);
        }

        #endregion

        #region Private methods

        private string CreateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtOptions.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(this.jwtOptions.Issuer,
                                             this.jwtOptions.Audience,
                                             expires: DateTime.Now.AddMinutes(30),
                                             signingCredentials: credentials);

            token.Payload.AddClaim(new Claim(CustomClaimTypes.UserId, user.UserId.ToString()));
            token.Payload.AddClaim(new Claim(CustomClaimTypes.UserName, $"{user.FirstName} {user.LastName}"));
            token.Payload.AddClaim(new Claim(CustomClaimTypes.UserEmail, $"{user.Email}"));
            token.Payload.AddClaim(new Claim(CustomClaimTypes.UserRole, $"{user.Role}"));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion

    }
}
