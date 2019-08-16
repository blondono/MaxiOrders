using System;
using System.Threading.Tasks;
using MaxiOrders.Back.Domain.Entities.Models;
using MaxiOrders.Back.Domain.Entities.Models.Response;
using MaxiOrders.Back.Domain.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace MaxiOrders.Back.WebApi.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        readonly IUserService _iUserService;

        public LoginController(IUserService iUserService)
        {
            _iUserService = iUserService;
        }

        [HttpPost]
        public async Task<ActionResult<Response<Auth>>> autenticate([FromBody] Login _objLogin)
        {
            return await _iUserService.Autenticate(_objLogin);
        }
    }
}