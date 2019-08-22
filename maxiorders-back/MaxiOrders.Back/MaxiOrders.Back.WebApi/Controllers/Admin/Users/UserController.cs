using System;
using System.Threading.Tasks;
using MaxiOrders.Back.Common.Enums;
using MaxiOrders.Back.Domain.Entities;
using MaxiOrders.Back.Domain.Entities.Models.Response;
using MaxiOrders.Back.Domain.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaxiOrders.Back.WebApi.Controllers.Admin.Users
{
    [Route("api/admin/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserService _iUserService;

        public UserController(IUserService iUserService)
        {
            _iUserService = iUserService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Response<User>>> Post([FromBody] User user)
        {
            Response<User> response = new Response<User>();
            try
            {
                _iUserService.Add(user);
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
            }
            catch (ApplicationException ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error al comprobar el usuario";
            }
            return response;
        }

        [HttpGet]
        public async Task<ActionResult<Response<User>>> Get()
        {
            Response<User> response = new Response<User>();
            try
            {
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
                response.List = await _iUserService.Get();
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error consultando la lista de usuarios";
            }
            return response;
        }
    }
}