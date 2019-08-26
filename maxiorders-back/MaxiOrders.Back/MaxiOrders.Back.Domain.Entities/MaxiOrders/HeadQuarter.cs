using System;
using System.Collections.Generic;

namespace MaxiOrders.Back.Domain.Entities
{
    public partial class HeadQuarter
    {
        public HeadQuarter()
        {
            Device = new HashSet<Device>();
        }
        public long IdHeadQuarter { get; set; }
        public long IdCompany { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public Company IdCompanyNavigation { get; set; }
        public ICollection<Device> Device { get; set; }
    }
}
