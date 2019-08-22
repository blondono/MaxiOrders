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
        Task<Auth> Auth(User login, bool validateAdmin);
        void Add(User user);
        Task<IEnumerable<User>> Get();
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

        public virtual async Task<Auth> Auth(User login, bool validateAdmin)
        {
            try
            {
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
                            Auth objAuth = new Auth();
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
                            objAuth.Token = token;
                            return objAuth;
                        }
                        else
                            throw new ApplicationException("No puede acceder a la parte administrativa");
                    }
                    else
                        throw new ApplicationException("Usuario y contraseña incorrectos");
                }
                else
                    throw new KeyNotFoundException("El usuario no existe");
            }
            catch (Exception ex)
            {
                throw new Exception("Error autenticando al usuario");
            }

        }

        public virtual async Task<IEnumerable<User>> Get()
        {
             return _iDBMaxiOrdersRepositories.Users.GetAll();
        }

        public virtual async void Add(User user)
        {
            try
            {
                User objUser = _iDBMaxiOrdersRepositories.Users.Get(x => x.Email.ToLower().Equals(user.Email.ToLower()));
                if (objUser == null)
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    user.Password = hashedPassword;
                    _iDBMaxiOrdersRepositories.Users.Add(user);
                    _iDBMaxiOrdersRepositories.Commit();
                }
                else
                    throw new ApplicationException(string.Format("el correo {0} ya existe", user.Email));
            }
            catch (Exception ex)
            {
                throw new Exception("Error al comprobar el usuario");
            }
        }
    }
}
