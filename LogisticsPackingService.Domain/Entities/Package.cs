using LogisticsPackingService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Domain.Entities
{
    public class Package
    {
        public required int Id { get; init; }

        public required Dimensions Dimensions { get; init; }

        public decimal Weight { get; init; }
    }
}
