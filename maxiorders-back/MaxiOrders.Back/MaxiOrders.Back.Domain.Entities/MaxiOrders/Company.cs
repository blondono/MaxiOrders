using System;
using System.Collections.Generic;

namespace MaxiOrders.Back.Domain.Entities
{
    public partial class Company
    {
        public Company()
        {
            HeadQuarter = new HashSet<HeadQuarter>();
        }

        public long IdCompany { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }

        public ICollection<HeadQuarter> HeadQuarter { get; set; }
    }
}
