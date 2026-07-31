using LogisticsPackingService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Domain.Entities
{
    public class Box
    {
        public required string Name { get; init; }

        public required Dimensions Dimensions { get; init; }

        public decimal MaxWeight { get; init; }
    }
}
