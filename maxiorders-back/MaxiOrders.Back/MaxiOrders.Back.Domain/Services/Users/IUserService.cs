using MaxiOrders.Back.Common.Enums;
using MaxiOrders.Back.Domain.Entities;
using MaxiOrders.Back.Domain.Entities.Models;
using MaxiOrders.Back.Domain.Entities.Models.Response;
using MaxiOrders.Back.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MaxiOrders.Back.Domain.Services.Users
{
    public interface IUserService
    {
        Task<Response<Auth>> Auth(User login, bool validateAdmin);
        Task<Response<User>> Add(User user);
        Task<Response<User>> Get();
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

        public virtual async Task<Response<Auth>> Auth(User login, bool validateAdmin)
        {
            Response<Auth> response = new Response<Auth>();
            try
            {
                Auth objAuth = new Auth();
                User objUser = _iDBMaxiOrdersRepositories.Users.Get(x => x.Email.ToLower().Equals(login.Email.ToLower()));
                if (objUser != null)
                {
                    bool validPassword = BCrypt.Net.BCrypt.Verify(login.Password, objUser.Password);

                    if (validPassword)
                    {
                        bool authorize = true;
                        if (validateAdmin)
                            authorize = (objUser.Role.Equals("ROLE_ADMIN") || objUser.Role.Equals("ROLE_SUPERADMIN"));

                        if (authorize)
                        {
                            objUser.Password = string.Empty;
                            objAuth.User = objUser;

                            var secretKey = _configuration["AppSettings:SecretKey"];
                            var key = Encoding.ASCII.GetBytes(secretKey);

                            var claims = new ClaimsIdentity(new Claim[]
                            {
                            new Claim(ClaimTypes.NameIdentifier, objUser.IdUser.ToString()),
                            new Claim(ClaimTypes.Email, objUser.Email)
                            });

                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = claims,
                                Expires = DateTime.UtcNow.AddDays(1),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                            };

                            var tokenHandler = new JwtSecurityTokenHandler();
                            var createdToken = tokenHandler.CreateToken(tokenDescriptor);

                            string token = tokenHandler.WriteToken(createdToken);
                            response.Code = EnumResponseCode.OK.GetHashCode();
                            response.Message = EnumResponseCode.OK.ToString();
                            objAuth.Token = token;
                        }
                        else
                        {
                            response.Code = EnumResponseCode.ServerError.GetHashCode();
                            response.Message = "No puede acceder a la parte administrativa";
                        }
                    }
                    else
                    {
                        response.Code = EnumResponseCode.ServerError.GetHashCode();
                        response.Message = "Usuario y contraseña incorrectos";
                    }
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
                response.Message = "Error autenticando al usuario";
            }
            return response;
        }

        public virtual async Task<Response<User>> Get()
        {

            Response<User> response = new Response<User>();
            try
            {
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
                response.List = _iDBMaxiOrdersRepositories.Users.GetAll();
            }
            catch(Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error consultando la lista de usuarios";
            }
            return response;
        }

        public virtual async Task<Response<User>> Add(User user)
        {
            Response<User> response = new Response<User>();
            try
            {
                User objUser = _iDBMaxiOrdersRepositories.Users.Get(x => x.Email.ToLower().Equals(user.Email.ToLower()));
                if (objUser == null)
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    user.Password = hashedPassword;
                    _iDBMaxiOrdersRepositories.Users.Add(user);
                    _iDBMaxiOrdersRepositories.Commit();
                    response.Code = EnumResponseCode.OK.GetHashCode();
                    response.Message = EnumResponseCode.OK.ToString();
                }
                else
                {
                    response.Code = EnumResponseCode.ServerError.GetHashCode();
                    response.Message = string.Format("el correo {0} ya existe", user.Email);
                }
                response.Content = null;
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
