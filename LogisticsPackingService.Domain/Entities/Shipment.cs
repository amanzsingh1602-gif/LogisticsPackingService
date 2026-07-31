using LogisticsPackingService.Domain.Common;
using LogisticsPackingService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Domain.Entities
{
    public class Shipment : BaseEntity
    {
        public string ShipmentNumber { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;

        public string Destination { get; set; } = string.Empty;

        public DateTime ShipmentDate { get; set; }

        public ShipmentStatus Status { get; set; }

        public ICollection<Package> Packages { get; set; } = new List<Package>();
    }
}
