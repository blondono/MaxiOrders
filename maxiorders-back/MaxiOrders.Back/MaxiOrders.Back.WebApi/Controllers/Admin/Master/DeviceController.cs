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
using Microsoft.Extensions.Configuration;

namespace MaxiOrders.Back.WebApi.Controllers.Admin.Master
{
    [Route("api/admin/device")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class DeviceController : ControllerBase
    {
        readonly IDeviceService _iDeviceService;
        private IHostingEnvironment _hostingEnvironment;
        private IConfiguration _configuration;

        public DeviceController(IDeviceService iDeviceService,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _iDeviceService = iDeviceService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<Response<Device>>> Get()
        {
            Response<Device> response = new Response<Device>();
            try
            {
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
                response.List = await _iDeviceService.Get();
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error consultando la lista de equipos";
            }
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Device>>> Get(long id)
        {
            Response<Device> response = new Response<Device>();
            try
            {
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
                response.Content = await _iDeviceService.Get(id);
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
        public async Task<IActionResult> GetImage(long id)
        {
            Device objDevice = _iDeviceService.Get(id).Result;
            if (objDevice != null)
            {
                if (!string.IsNullOrEmpty(objDevice.Image))
                {
                    string folderName = _configuration["AppSettings:DeviceFolder"];
                    string webRootPath = _hostingEnvironment.ContentRootPath;
                    string path = Path.Combine(webRootPath, folderName, objDevice.Image);
                    var image = System.IO.File.OpenRead(path);

                    return File(image, "image/jpeg");
                }
                else return null;
            }
            else return null;
        }

        [HttpGet("{id}/{file}")]
        [Route("download/{id}/{file}")]
        public async Task<IActionResult> Download(long id, string file)
        {
            try
            {
            Device objDevice = _iDeviceService.Get(id).Result;
            if (objDevice != null)
            {
                string filename = string.Empty;
                switch (file)
                {
                    case "image":
                        filename = objDevice.Image;
                        break;
                    case "billimage":
                        filename = objDevice.BillImage;
                        break;
                    case "datasheets":
                        filename = objDevice.DataSheets;
                        break;
                }

                if (!string.IsNullOrEmpty(filename))
                {
                    string folderName = _configuration["AppSettings:DeviceFolder"];
                        string webRootPath = _hostingEnvironment.ContentRootPath;
                    string path = Path.Combine(webRootPath, folderName);
                
                    IFileProvider provider = new PhysicalFileProvider(path);
                    IFileInfo fileInfo = provider.GetFileInfo(filename);
                    var readStream = fileInfo.CreateReadStream();
                    var mimeType = "application/octet-stream";
                    return File(readStream, mimeType, filename);
                }
                else return null;
            }
            else return null;

            }catch(Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Response<Device>>> Post([FromBody] Device device)
        {
            Response<Device> response = new Response<Device>();
            try
            {
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
                response.Content = await _iDeviceService.Add(device);
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
        public async Task<ActionResult<Response<Device>>> Put(int id, [FromBody] Device device)
        {
            Response<Device> response = new Response<Device>();
            try
            {
                _iDeviceService.Update(device);
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
        public async Task<ActionResult<Response<Device>>> Delete(long id)
        {
            Response<Device> response = new Response<Device>();
            try
            {
                _iDeviceService.Delete(id);
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
        public async Task<ActionResult<Response<Device>>> Upload(long id, [FromForm]IEnumerable<IFormFile> files)
        {
            Response<Device> response = new Response<Device>();
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    string folderName = _configuration["AppSettings:DeviceFolder"];
                    string webRootPath = _hostingEnvironment.ContentRootPath;
                    string newPath = Path.Combine(webRootPath, folderName);
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    Device objDevice = await _iDeviceService.Get(id);
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
                            switch (item.Name)
                            {
                                case "image":
                                    objDevice.Image = item.FileName;
                                    break;
                                case "billimage":
                                    objDevice.BillImage = item.FileName;
                                    break;
                                case "datasheets":
                                    objDevice.DataSheets = item.FileName;
                                    break;
                            }
                        }
                    }
                    _iDeviceService.Update(objDevice);
                    response.Code = EnumResponseCode.OK.GetHashCode();
                    response.Message = EnumResponseCode.OK.ToString();
                    response.Content = objDevice;
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