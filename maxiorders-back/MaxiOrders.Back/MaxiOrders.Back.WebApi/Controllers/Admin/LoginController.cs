using System;
using System.Threading.Tasks;
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
            return await _iUserService.Auth(login, true);
        }
    }
}