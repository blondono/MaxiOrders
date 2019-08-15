using System;
using System.Collections.Generic;

namespace MaxiOrders.Back.Domain.Entities
{
    public partial class User
    {
        public long IdUser { get; set; }
        public string Identification { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Photo { get; set; }
        public string DigitalSignature { get; set; }
    }
}
