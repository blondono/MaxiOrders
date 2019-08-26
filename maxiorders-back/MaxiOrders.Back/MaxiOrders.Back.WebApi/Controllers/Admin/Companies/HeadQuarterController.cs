using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MaxiOrders.Back.Common.Enums;
using MaxiOrders.Back.Domain.Entities;
using MaxiOrders.Back.Domain.Entities.Models.Response;
using MaxiOrders.Back.Domain.Services.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using MaxiOrders.Back.Domain.Services.Companies;
using Microsoft.Extensions.Configuration;

namespace MaxiOrders.Back.WebApi.Controllers.Admin.Companies
{
    [Route("api/admin/headquarter")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class HeadQuarterController : ControllerBase
    {
        readonly IHeadQuarterService _service;
        private IHostingEnvironment _hostingEnvironment;
        private IConfiguration _configuration;

        public HeadQuarterController(IHeadQuarterService service,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _service = service;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<Response<HeadQuarter>>> Get()
        {
            Response<HeadQuarter> response = new Response<HeadQuarter>();
            try
            {
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
                response.List = await _service.Get();
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error consultando la lista de equipos";
            }
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<HeadQuarter>>> Get(long id)
        {
            Response<HeadQuarter> response = new Response<HeadQuarter>();
            try
            {
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
                response.Content = await _service.Get(id);
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error consultando la lista de equipos";
            }
            return response;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Response<HeadQuarter>>> Post([FromBody] HeadQuarter HeadQuarter)
        {
            Response<HeadQuarter> response = new Response<HeadQuarter>();
            try
            {
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
                response.Content = await _service.Add(HeadQuarter);
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error al guardar el equipo";
            }
            return response;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Response<HeadQuarter>>> Put(int id, [FromBody] HeadQuarter HeadQuarter)
        {
            Response<HeadQuarter> response = new Response<HeadQuarter>();
            try
            {
                _service.Update(HeadQuarter);
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error al actualizar el equipo";
            }
            return response;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Response<HeadQuarter>>> Delete(long id)
        {
            Response<HeadQuarter> response = new Response<HeadQuarter>();
            try
            {
                _service.Delete(id);
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error al guardar el equipo";
            }
            return response;
        }
    }
}