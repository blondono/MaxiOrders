using MaxiOrders.Back.Common.Enums;
using MaxiOrders.Back.Domain.Entities;
using MaxiOrders.Back.Domain.Entities.Models;
using MaxiOrders.Back.Domain.Entities.Models.Response;
using MaxiOrders.Back.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MaxiOrders.Back.Domain.Services.Users
{
    public interface IUserService
    {
        Task<Response<Auth>> Autenticate(Login login);
    }

    public class UserService : IUserService
    {
        private readonly IDBMaxiOrdersRepositories _iDBMaxiOrdersRepositories;
        private readonly IConfiguration _configuration;
        public UserService(IDBMaxiOrdersRepositories iDBMaxiOrdersRepositories,
            IConfiguration configuration)
        {
            _iDBMaxiOrdersRepositories = iDBMaxiOrdersRepositories;
            _configuration = configuration;
        }

        public virtual async Task<Response<Auth>> Autenticate(Login login)
        {
            Response<Auth> response = new Response<Auth>();
            try
            {
                Auth objAuth = new Auth();
                User objUser = _iDBMaxiOrdersRepositories.Users.Get(x => x.Name.ToLower().Equals(login.UserName.ToLower()));
                if (objUser != null)
                {
                    objUser.Password = string.Empty;
                    objAuth.User = objUser;
                    // authentication successful so generate jwt token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:SecretKey"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, objUser.IdUser.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    response.Code = EnumResponseCode.OK.GetHashCode();
                    response.Message = EnumResponseCode.OK.ToString();
                    objAuth.Token = tokenHandler.WriteToken(token);
                }
                else
                {
                    response.Code = EnumResponseCode.ServerError.GetHashCode();
                    response.Message = "El usuario no existe";
                }
                response.Content = objAuth;
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error al comprobar el usuario";
            }
            return response;
        }
    }
}
