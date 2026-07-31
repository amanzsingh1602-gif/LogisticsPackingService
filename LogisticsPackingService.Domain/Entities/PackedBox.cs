using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Domain.Entities
{
    public sealed class PackedBox
    {
        public required Box Box { get; init; }

        public List<Package> Packages { get; } = new();

        public decimal RemainingWeight { get; set; }
    }
}
