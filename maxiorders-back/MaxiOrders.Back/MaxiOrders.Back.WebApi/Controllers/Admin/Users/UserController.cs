using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaxiOrders.Back.Domain.Entities;
using MaxiOrders.Back.Domain.Entities.Models;
using MaxiOrders.Back.Domain.Entities.Models.Response;
using MaxiOrders.Back.Domain.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MaxiOrders.Back.WebApi.Controllers.Admin.Users
{
    [Route("api/admin/user")]
    [Authorize]
    public class UserController : ControllerBase
    {
        readonly IUserService _iUserService;

        public UserController(IUserService iUserService)
        {
            _iUserService = iUserService;
        }

        [HttpPost]
        public async Task<ActionResult<Response<User>>> Post([FromBody] User user)
        {
            return await _iUserService.Add(user);
        }

        [HttpGet]
        public async Task<ActionResult<Response<User>>> Get()
        {
            return await _iUserService.Get();
        }
    }
}