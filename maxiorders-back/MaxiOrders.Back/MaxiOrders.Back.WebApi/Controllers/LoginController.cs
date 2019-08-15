using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MaxiOrders.Back.WebApi.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [Route("autenticate")]
        public async Task<ActionResult<OrderSaveResponse>> Save([FromBody] OrderDataEntity orderDataEntity)
        {
        }
}