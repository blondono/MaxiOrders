using System;
using System.Collections.Generic;
using MaxiOrders.Back.Domain.Entities;

namespace MaxiOrders.Back.Domain.Entities.Models
{
    public class Auth
    {
        public string Token { get; set; }
        public User User { get; set; }
    }
}
