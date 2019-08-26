using System;
using System.Collections.Generic;

namespace MaxiOrders.Back.Domain.Entities
{
    public partial class Device
    {
        public long IdDevice { get; set; }
        public long IdHeadQuarter { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Serie { get; set; }
        public string LicenseNumber { get; set; }
        public bool State { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime InstalationDate { get; set; }
        public string Image { get; set; }
        public string BillImage { get; set; }
        public string DataSheets { get; set; }
        public HeadQuarter IdHeadQuarterNavigation { get; set; }
    }
}
