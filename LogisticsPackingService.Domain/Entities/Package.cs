using LogisticsPackingService.Domain.Common;
using LogisticsPackingService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Domain.Entities
{
    public class Package : BaseEntity
    {
        public string PackageNumber { get; set; } = string.Empty;

        public PackageType PackageType { get; set; }

        public decimal Weight { get; set; }

        public decimal Length { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }

        // FK
        public Guid ShipmentId { get; set; }

        // For Navigation
        public Shipment Shipment { get; set; } = null!;

        public Guid? ContainerId { get; set; }

        public Container? Container { get; set; }
    }
}
