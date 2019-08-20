
using System.Collections.Generic;

namespace MaxiOrders.Back.Domain.Entities.Models.Response
{
    public class Response<T> where T : class
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Content { get; set; }
        public IEnumerable<T> List { get; set; }
    }
}
