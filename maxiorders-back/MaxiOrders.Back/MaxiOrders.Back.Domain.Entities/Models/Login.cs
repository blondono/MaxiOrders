using System;
using System.Collections.Generic;
using System.Text;

namespace MaxiOrders.Back.Domain.Entities.Models
{
    public class Login
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
