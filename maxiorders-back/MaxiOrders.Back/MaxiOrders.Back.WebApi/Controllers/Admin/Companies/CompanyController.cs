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
    [Route("api/admin/company")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class CompanyController : ControllerBase
    {
        readonly ICompanyService _service;
        private IHostingEnvironment _hostingEnvironment;
        private IConfiguration _configuration;

        public CompanyController(ICompanyService service,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _service = service;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<Response<Company>>> Get()
        {
            Response<Company> response = new Response<Company>();
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
        public async Task<ActionResult<Response<Company>>> Get(long id)
        {
            Response<Company> response = new Response<Company>();
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

        [HttpGet("{id}")]
        [Route("image/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            Company objCompany = _service.Get(id).Result;
            if (objCompany != null)
            {
                if (!string.IsNullOrEmpty(objCompany.Logo))
                {
                    string folderName = _configuration["AppSettings:CompanyFolder"];
                    string webRootPath = _hostingEnvironment.ContentRootPath;
                    string path = Path.Combine(webRootPath, folderName, objCompany.Logo);
                    var image = System.IO.File.OpenRead(path);

                    return File(image, "image/jpeg");
                }
                else return null;
            }
            else return null;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Response<Company>>> Post([FromBody] Company Company)
        {
            Response<Company> response = new Response<Company>();
            try
            {
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
                response.Content = await _service.Add(Company);
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
        public async Task<ActionResult<Response<Company>>> Put(int id, [FromBody] Company Company)
        {
            Response<Company> response = new Response<Company>();
            try
            {
                _service.Update(Company);
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
        public async Task<ActionResult<Response<Company>>> Delete(long id)
        {
            Response<Company> response = new Response<Company>();
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

        [HttpPost, DisableRequestSizeLimit]
        [Route("upload/{id}")]
        [Authorize]
        public async Task<ActionResult<Response<Company>>> Upload(long id, [FromForm]IEnumerable<IFormFile> files)
        {
            Response<Company> response = new Response<Company>();
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    string folderName = _configuration["AppSettings:CompanyFolder"];
                    string webRootPath = _hostingEnvironment.ContentRootPath;
                    string newPath = Path.Combine(webRootPath, folderName);
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    Company objCompany = await _service.Get(id);
                    foreach (IFormFile item in Request.Form.Files)
                    {
                        if (item.Length > 0)
                        {
                            string fileName = ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"');
                            string fullPath = Path.Combine(newPath, fileName);
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                item.CopyTo(stream);
                            }
                            if(item.Name.Equals("image"))
                                    objCompany.Logo = item.FileName;
                        }
                    }
                    _service.Update(objCompany);
                    response.Code = EnumResponseCode.OK.GetHashCode();
                    response.Message = EnumResponseCode.OK.ToString();
                    response.Content = objCompany;
                }
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = "No hay archivos para subir";
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = "Error al subir el archivo, error: " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            return response;
        }
    }
}