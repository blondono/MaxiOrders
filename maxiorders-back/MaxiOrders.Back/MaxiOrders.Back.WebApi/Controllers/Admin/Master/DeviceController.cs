using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MaxiOrders.Back.Domain.Entities;
using MaxiOrders.Back.Domain.Entities.Models.Response;
using MaxiOrders.Back.Domain.Services.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaxiOrders.Back.WebApi.Controllers.Admin.Master
{
    [Route("api/admin/device")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class DeviceController : ControllerBase
    {
        readonly IDeviceService _iDeviceService;
        private IHostingEnvironment _hostingEnvironment;

        public DeviceController(IDeviceService iDeviceService,
            IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _iDeviceService = iDeviceService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Response<Device>>> Post([FromBody] Device device)
        {
            return await _iDeviceService.Add(device);
        }

        [HttpGet]
        public async Task<ActionResult<Response<Device>>> Get()
        {
            return await _iDeviceService.Get();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Device>>> Get(long id)
        {
            return await _iDeviceService.Get();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Response<Device>>> Put(int id, [FromBody] Device device)
        {
            return await _iDeviceService.Update(device);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Response<Device>>> Delete(long id)
        {
            return await _iDeviceService.Delete(id);
        }

        [HttpPost]
        public async Task<ActionResult> UploadImage(List<IFormFile> files)
        {
            if (files != null && files.Count > 0)
            {
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                foreach (IFormFile item in files)
                {
                    if (item.Length > 0)
                    {
                        string fileName = ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"');
                        string fullPath = Path.Combine(newPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }
                    }
                }
                return this.Content("Success");
            }
            return this.Content("Fail");
        }
    }
    }
}