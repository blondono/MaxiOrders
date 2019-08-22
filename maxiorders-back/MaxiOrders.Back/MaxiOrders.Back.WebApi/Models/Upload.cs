using Microsoft.AspNetCore.Http;

namespace MaxiOrders.Back.WebApi.Models
{
    public class Upload
    {
        public string Field { get; set; }
        public IFormFile File { get; set; }
    }
}
