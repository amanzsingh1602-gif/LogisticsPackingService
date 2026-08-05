using System;
using System.Collections.Generic;
using System.Text;

namespace LogisticsPackingService.Domain.Entities
{
    public sealed class Shelf
    {
        public decimal Height { get; set; }

        public decimal RemainingLength { get; set; }

        public List<Package> Packages { get; } = new();
    }
}
