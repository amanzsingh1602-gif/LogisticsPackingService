using LogisticsPackingService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Domain.Entities
{
    public class Container : BaseEntity
    {
        public string ContainerNumber { get; set; } = string.Empty;

        public string ContainerType { get; set; } = string.Empty;

        public decimal MaxWeight { get; set; }

        public decimal CurrentWeight { get; set; }

        public ICollection<Package> Packages { get; set; } = new List<Package>();
    }
}
