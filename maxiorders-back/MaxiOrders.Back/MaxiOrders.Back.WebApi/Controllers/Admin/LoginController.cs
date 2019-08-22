using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MaxiOrders.Back.Common.Enums;
using MaxiOrders.Back.Domain.Entities;
using MaxiOrders.Back.Domain.Entities.Models;
using MaxiOrders.Back.Domain.Entities.Models.Response;
using MaxiOrders.Back.Domain.Services.Users;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MaxiOrders.Back.WebApi.Controllers.Admin
{
    [Route("api/admin/login")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class LoginController : ControllerBase
    {
        readonly IUserService _iUserService;

        public LoginController(IUserService iUserService)
        {
            _iUserService = iUserService;
        }

        [HttpPost]
        public async Task<ActionResult<Response<Auth>>> Post([FromBody] User login)
        {
            Response<Auth> response = new Response<Auth>();
            try
            {
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
                response.Content =  await _iUserService.Auth(login, true);
            }
            catch(KeyNotFoundException ex)
            {
                response.Code = EnumResponseCode.NotFound.GetHashCode();
                response.Message = ex.Message;
            }
            catch(ApplicationException ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error autenticando al usuario";
            }
            return response;
        }
    }
}